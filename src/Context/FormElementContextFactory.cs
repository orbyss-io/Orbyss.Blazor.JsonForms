using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Context.Models;
using Orbyss.Components.JsonForms.Interpretation;
using Orbyss.Components.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Components.JsonForms.Context
{
    public sealed class FormElementContextFactory(IJsonPathInterpreter jsonPathInterpreter) : IFormElementContextFactory
    {
        public IFormElementContext Create(IUiSchemaElementInterpretation interpretation, string? parentAbsoluteDataJsonPath)
        {
            return interpretation.ElementType switch
            {
                UiSchemaElementInterpretationType.Control => CreateControl((UiSchemaControlInterpretation)interpretation, parentAbsoluteDataJsonPath),
                UiSchemaElementInterpretationType.VerticalLayout => CreateVerticalLayout((UiSchemaVerticalLayoutInterpretation)interpretation, parentAbsoluteDataJsonPath),
                UiSchemaElementInterpretationType.HorizontalLayout => CreateHorizontalLayout((UiSchemaHorizontalLayoutInterpretation)interpretation, parentAbsoluteDataJsonPath),
                UiSchemaElementInterpretationType.List => CreateList((UiSchemaListInterpretation)interpretation, parentAbsoluteDataJsonPath),
                _ => throw new NotSupportedException($"Element type '{interpretation.ElementType}' is not supported.")
            };
        }

        public FormPageContext[] CreatePages(UiSchemaPageInterpretation[] pageInterpretations)
        {
            var result = new FormPageContext[pageInterpretations.Length];

            for (var j = 0; j < pageInterpretations.Length; j++)
            {
                var page = pageInterpretations[j];
                var pageElementContexts = new IFormElementContext[page.InterpretedElements.Length];

                for (var i = 0; i < page.InterpretedElements.Length; i++)
                {
                    var pageElementInterpretation = page.InterpretedElements[i];
                    var elementContext = Create(
                        pageElementInterpretation,
                        parentAbsoluteDataJsonPath: null
                    );
                    pageElementContexts[i] = elementContext;
                }

                result[j] = new FormPageContext(page, pageElementContexts);
            }

            return result;
        }

        FormControlContext CreateControl(UiSchemaControlInterpretation controlInterpretation, string? parentElementAbsoluteDataPath)
        {
            if (!string.IsNullOrWhiteSpace(parentElementAbsoluteDataPath))
            {
                var relativeJsonDataPath = jsonPathInterpreter.FromJsonSchemaPath(controlInterpretation.RelativeSchemaJsonPath);
                var absoluteDataPath = jsonPathInterpreter.JoinJsonPaths(parentElementAbsoluteDataPath, relativeJsonDataPath);
                var parentDataPath = jsonPathInterpreter.GetParentPathFromDataPath(absoluteDataPath);

                return new FormControlContext(
                    absoluteDataPath,
                    parentDataPath,
                    controlInterpretation
                );
            }

            var absoluteJsonDataPath = jsonPathInterpreter.FromJsonSchemaPath(controlInterpretation.AbsoluteSchemaJsonPath);
            var parentDataJsonPath = jsonPathInterpreter.GetParentPathFromDataPath(absoluteJsonDataPath);

            return new FormControlContext(
                absoluteJsonDataPath,
                parentDataJsonPath,
                controlInterpretation
            );
        }


        FormListContext CreateList(UiSchemaListInterpretation listInterpretation, string? parentAbsoluteDataPath)
        {
            if (!string.IsNullOrWhiteSpace(parentAbsoluteDataPath))
            {
                var relativeJsonDataPath = jsonPathInterpreter.FromJsonSchemaPath(listInterpretation.RelativeSchemaJsonPath);
                var absoluteDataPath = jsonPathInterpreter.JoinJsonPaths(parentAbsoluteDataPath, relativeJsonDataPath);
                var absoluteParentDataPath = jsonPathInterpreter.GetParentPathFromDataPath(absoluteDataPath);

                return new FormListContext(
                    listInterpretation,
                    absoluteDataPath,
                    absoluteParentDataPath
                );
            }

            var absoluteJsonDataPath = jsonPathInterpreter.FromJsonSchemaPath(listInterpretation.AbsoluteSchemaJsonPath);
            var absoluteParentDataJsonPath = jsonPathInterpreter.GetParentPathFromDataPath(absoluteJsonDataPath);
            return new FormListContext(
                listInterpretation,
                absoluteJsonDataPath,
                absoluteParentDataJsonPath
            );
        }

        FormVerticalLayoutContext CreateVerticalLayout(UiSchemaVerticalLayoutInterpretation interpretation, string? parentAbsoluteDataPath)
        {
            var rows = interpretation.Rows.Select(row =>
            {
                return Create(row, parentAbsoluteDataPath);
            })
            .ToArray();

            return new FormVerticalLayoutContext(interpretation, rows);
        }

        FormHorizontalLayoutContext CreateHorizontalLayout(UiSchemaHorizontalLayoutInterpretation interpretation, string? parentAbsoluteDataPath)
        {
            var columns = interpretation.Columns.Select(column =>
            {
                return Create(column, parentAbsoluteDataPath);
            })
            .ToArray();

            return new FormHorizontalLayoutContext(interpretation, columns);
        }
    }
}
