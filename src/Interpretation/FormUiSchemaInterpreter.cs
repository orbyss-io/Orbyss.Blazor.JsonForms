using Newtonsoft.Json.Schema;
using Orbyss.Components.JsonForms.Helpers;
using Orbyss.Components.JsonForms.Interpretation.Interfaces;
using Orbyss.Components.JsonForms.UiSchema;
using Orbyss.Components.JsonForms.Utils;


namespace Orbyss.Components.JsonForms.Interpretation
{
    public sealed class FormUiSchemaInterpreter(IJsonPathInterpreter jsonPathInterpreter, IControlTypeInterpreter controlTypeInterpreter) : IFormUiSchemaInterpreter
    {
        public UiSchemaInterpretation Interpret(FormUiSchema uiSchema, JSchema jsonSchema)
        {            
            if (uiSchema.Type == UiSchemaElementType.Categorization)
            {
                return new UiSchemaInterpretation(
                    uiSchema,
                    pages: InterpretCategories(uiSchema, jsonSchema)
                );
            }

            var rootFormUiSchemaElement = new FormUiSchemaElement(
                uiSchema.Type,
                uiSchema.Label,
                I18n: null,
                uiSchema.Elements,
                uiSchema.Scope,
                Rule: null,
                uiSchema.Options
            );

            var rootElement = Interpret(rootFormUiSchemaElement, jsonSchema, null);
            var rootPage = new UiSchemaPageInterpretation(
                IsReadOnly(rootFormUiSchemaElement),
                IsDisabled(rootFormUiSchemaElement),
                IsHidden(rootFormUiSchemaElement),
                rootElement,
                null,
                GetRule(rootFormUiSchemaElement)
            );

            return new UiSchemaInterpretation(
                uiSchema,
                page: rootPage
            );
        }

        private UiSchemaPageInterpretation[] InterpretCategories(FormUiSchema uiSchema, JSchema jsonSchema)
        {
            if (uiSchema.Elements.All(x => x.Type == UiSchemaElementType.Category) == false)
            {
                throw new InvalidOperationException($"For a UI Schema of type categorization, all direct child elements must be of type Category");
            }

            var pages = new List<UiSchemaPageInterpretation>();

            foreach (var categoryElement in uiSchema.Elements)
            {
                var pageElements = InterpretUiSchemaLayouts(categoryElement.Elements, jsonSchema);
                var page = new UiSchemaPageInterpretation(
                    IsReadOnly(categoryElement),
                    IsDisabled(categoryElement),
                    IsHidden(categoryElement),
                    pageElements,
                    (UiSchemaLabelInterpretation)categoryElement,
                    GetRule(categoryElement)
                );
                pages.Add(page);
            }

            return [.. pages];
        }

        private IUiSchemaElementInterpretation[] InterpretUiSchemaLayouts(FormUiSchemaElement[] elements, JSchema jsonSchema)
        {
            var result = new IUiSchemaElementInterpretation[elements.Length];
            for (var i = 0; i < elements.Length; i++)
            {
                result[i] = Interpret(elements[i], jsonSchema, null);
            }
            return result;
        }

        IUiSchemaElementInterpretation Interpret(FormUiSchemaElement element, JSchema jsonSchema, string? parentAbsoluteSchemaJsonPath)
        {
            return element.Type switch
            {
                UiSchemaElementType.HorizontalLayout => InterpretHorizontalLayout(element,jsonSchema, parentAbsoluteSchemaJsonPath),
                UiSchemaElementType.VerticalLayout => InterpretVerticalLayout(element, jsonSchema, parentAbsoluteSchemaJsonPath),
                UiSchemaElementType.Group => InterpretVerticalLayout(element, jsonSchema, parentAbsoluteSchemaJsonPath),
                UiSchemaElementType.Control => InterpretControl(element, jsonSchema, parentAbsoluteSchemaJsonPath),
                UiSchemaElementType.ListWithDetail => InterpretList(element, jsonSchema, parentAbsoluteSchemaJsonPath),
                _ => throw new NotSupportedException()
            };
        }

        private UiSchemaHorizontalLayoutInterpretation InterpretHorizontalLayout(FormUiSchemaElement horizontalLayoutElement, JSchema jsonSchema, string? parentAbsoluteSchemaJsonPath)
        {
            if (!horizontalLayoutElement.HasChildElements)
                throw new InvalidOperationException("Horizontal layout element must have elements defined");

            var result = new UiSchemaHorizontalLayoutInterpretation();

            var columns = horizontalLayoutElement.Elements
                .Select(x => Interpret(x, jsonSchema, parentAbsoluteSchemaJsonPath))
                .ToArray();
            result.SetColumns(columns);

            return result;
        }

        private UiSchemaVerticalLayoutInterpretation InterpretVerticalLayout(FormUiSchemaElement verticalLayoutElement, JSchema jsonSchema, string? parentAbsoluteSchemaJsonPath)
        {
            if (!verticalLayoutElement.HasChildElements)
                throw new InvalidOperationException("Vertical layout element must have elements defined");

            var result = new UiSchemaVerticalLayoutInterpretation((UiSchemaLabelInterpretation)verticalLayoutElement);

            var rows = verticalLayoutElement.Elements
                .Select(X => Interpret(X, jsonSchema, parentAbsoluteSchemaJsonPath))
                .ToArray();
            result.SetRows(rows);

            return result;
        }

        private UiSchemaControlInterpretation InterpretControl(FormUiSchemaElement primitiveControlElement, JSchema jsonSchema, string? parentAbsoluteSchemaJsonPath)
        {
            if (primitiveControlElement.HasChildElements)
                throw new InvalidOperationException("Elements of type 'Control' cannot have child elements");

            var elementSchemaJsonPath = jsonPathInterpreter.FromElementScope(primitiveControlElement.Scope);
            var absoluteSchemaJsonPath = !string.IsNullOrWhiteSpace(parentAbsoluteSchemaJsonPath)
                ? jsonPathInterpreter.JoinJsonPaths(parentAbsoluteSchemaJsonPath, elementSchemaJsonPath)
                : elementSchemaJsonPath;
            var controlJsonPropertyName = jsonPathInterpreter.GetJsonPropertyNameFromPath(absoluteSchemaJsonPath);
            var absoluteParentObjectSchemaPath = jsonPathInterpreter.GetParentPathFromSchemaPath(absoluteSchemaJsonPath);
            var controlType = controlTypeInterpreter.Interpret(jsonSchema, absoluteSchemaJsonPath, absoluteParentObjectSchemaPath);

            return new UiSchemaControlInterpretation(
                controlType,
                (UiSchemaLabelInterpretation)primitiveControlElement,
                IsReadOnly(primitiveControlElement),
                IsDisabled(primitiveControlElement),
                IsHidden(primitiveControlElement),
                elementSchemaJsonPath,
                absoluteSchemaJsonPath,
                controlJsonPropertyName,
                absoluteParentObjectSchemaPath,
                primitiveControlElement,
                GetRule(primitiveControlElement)
            );
        }

        private UiSchemaRuleInterpretation? GetRule(FormUiSchemaElement element)
        {
            if (element.Rule is not null)
            {
                return new UiSchemaRuleInterpretation(
                    jsonPathInterpreter.FromElementScope(element.Rule.Condition.Scope),
                    JSchema.Parse(
                        ObjectJsonConverter.Serialize(element.Rule.Condition.Schema)
                    ),
                    element.Rule.Effect
                );
            }

            return null;
        }

        private UiSchemaListInterpretation InterpretList(FormUiSchemaElement listWithDetailsElement, JSchema jsonSchema, string? parentAbsoluteSchemaJsonPath)
        {
            if (!listWithDetailsElement.HasOption(FormUiSchemaOptionKeys.Detail))
                throw new InvalidOperationException("ListWithDetails element must have options.detail defined");

            var listScope = listWithDetailsElement.Scope;
            var listItemsScope = string.Concat(listWithDetailsElement.Scope, "/items");

            var listSchemaJsonPath = jsonPathInterpreter.FromElementScope(listScope);
            var listItemsSchemaJsonPath = jsonPathInterpreter.FromElementScope(listItemsScope);

            var absoluteListSchemaJsonPath = !string.IsNullOrWhiteSpace(parentAbsoluteSchemaJsonPath)
                ? jsonPathInterpreter.JoinJsonPaths(parentAbsoluteSchemaJsonPath, listSchemaJsonPath)
                : listSchemaJsonPath;
            var absoluteListItemSchemaJsonPath = !string.IsNullOrWhiteSpace(parentAbsoluteSchemaJsonPath)
                ? jsonPathInterpreter.JoinJsonPaths(parentAbsoluteSchemaJsonPath, listItemsSchemaJsonPath)
                : listItemsSchemaJsonPath;
            var jsonPropertyName = jsonPathInterpreter.GetJsonPropertyNameFromPath(absoluteListSchemaJsonPath);
            var absoluteParentObjectSchemaPath = jsonPathInterpreter.GetParentPathFromSchemaPath(absoluteListSchemaJsonPath);

            var list = new UiSchemaListInterpretation(
                (UiSchemaLabelInterpretation)listWithDetailsElement,
                IsReadOnly(listWithDetailsElement),
                IsDisabled(listWithDetailsElement),
                IsHidden(listWithDetailsElement),
                listSchemaJsonPath,
                absoluteListSchemaJsonPath,
                listItemsSchemaJsonPath,
                absoluteListItemSchemaJsonPath,
                jsonPropertyName,
                absoluteParentObjectSchemaPath,
                listWithDetailsElement,
                GetRule(listWithDetailsElement)
            );

            var detailOption = listWithDetailsElement.GetOption(FormUiSchemaOptionKeys.Detail);
            var detailElement = DefaultJsonConverter.Deserialize<FormUiSchemaElement>($"{detailOption}");
            var detailInterpretation = Interpret(detailElement, jsonSchema, absoluteListItemSchemaJsonPath);

            list.SetListDetail(detailInterpretation);

            return list;
        }

        

        private static bool IsDisabled(FormUiSchemaElement element)
        {
            return IsBooleanOptionValue(element, FormUiSchemaOptionKeys.Disabled);
        }

        private static bool IsHidden(FormUiSchemaElement element)
        {
            return IsBooleanOptionValue(element, FormUiSchemaOptionKeys.Hidden);
        }

        private static bool IsReadOnly(FormUiSchemaElement element)
        {
            return IsBooleanOptionValue(element, FormUiSchemaOptionKeys.ReadOnly);
        }

        static bool IsBooleanOptionValue(FormUiSchemaElement element, string option)
        {
            return element.HasOption(option)
                && bool.Parse($"{element.GetOption(option)}");
        }
    }
}
