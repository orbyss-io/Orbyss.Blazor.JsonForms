using Newtonsoft.Json.Linq;
using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Context.Models;
using Orbyss.Components.JsonForms.Context.Notifications;
using Orbyss.Components.JsonForms.Helpers;
using Orbyss.Components.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Components.JsonForms.Context
{
    public sealed class JsonFormContext(
        IJsonFormNotificationHandler notificationHandler,
        IFormUiSchemaInterpreter uiSchemaInterpreter,
        IFormContextFactory elementContextFactory,        
        IJsonFormDataContext dataContext,
        IJsonFormTranslationContext translationContext,
        IFormRuleEnforcer ruleEnforcer
    )
        : IJsonFormContext
    {
        private FormPageContext[] pages = [];
        private string? activeLanguage;
        private JObject options = [];

        private bool disabled;
        private bool readOnly;

        public IEnumerable<FormPageContext> GetPages() => pages;

        public string? ActiveLanguage => activeLanguage;

        public IJsonFormNotification FormNotification => notificationHandler;

        public int PageCount => pages.Length;

        public bool Disabled => disabled;

        public bool ReadOnly => readOnly;

        public void Instantiate(JsonFormContextInitOptions initOptions)
        {
            if (pages.Length > 0)
            {
                throw new InvalidOperationException("Context is already instantiated");
            }

            var data = initOptions.Data ?? new JObject();
            var dataSchema = initOptions.DataSchema;
            var translationSchema = initOptions.TranslationSchema;
            var uiSchema = initOptions.UiSchema;

            dataContext.Instantiate(data, dataSchema);
            translationContext.Instantiate(translationSchema, dataSchema);

            disabled = initOptions.Disabled;
            activeLanguage = initOptions.Language;
            readOnly = initOptions.ReadOnly;

            var uiSchemaInterpretation = uiSchemaInterpreter.Interpret(uiSchema, dataSchema);
            options = uiSchema.Options?.ToJToken() as JObject ?? [];
            pages = elementContextFactory.CreatePages(uiSchemaInterpretation.Pages);

            EnforceRules();
        }

        public JToken? GetFormOption(string key)
        {
            if (options.ContainsKey(key))
            {
                return options[key];
            }

            return null;
        }

        public bool Validate(Guid? pageId = null)
        {
            var contextsToValidate = pageId.HasValue
                ? pages.FirstOrDefault(x => x.Id == pageId.Value)?.ElementContexts ?? throw new InvalidOperationException($"Page with id '{pageId}' does not exist")
                : pages.SelectMany(x => x.ElementContexts);

            var result = dataContext.Validate(contextsToValidate);

            notificationHandler.Notify(JsonFormNotificationType.OnDataValidated);

            return result;
        }

        public JToken? GetValue(Guid dataContextId)
        {
            var match = FindContextById(dataContextId);
            var dataElement = CastControl(match);
            return dataContext.GetValue(dataElement);
        }

        public void UpdateValue(Guid dataContextId, JToken? value)
        {
            var match = FindContextById(dataContextId);
            var dataElement = CastControl(match);
            dataContext.UpdateValue(dataElement, value);
            EnforceRules();
            notificationHandler.Notify(JsonFormNotificationType.OnDataChanged);
        }

        public string? GetDataContextError(Guid dataContextId)
        {
            var match = FindContextById(dataContextId);
            if (match is FormListContext list && list.Errors.Any())
            {
                return translationContext.TranslateErrors(ActiveLanguage, list.Errors, list.Interpretation);
            }

            if (match is FormControlContext control && control.Errors.Any())
            {
                return translationContext.TranslateErrors(ActiveLanguage, control.Errors, control.Interpretation);
            }

            return null;
        }

        public string? GetLabel(Guid contextId)
        {
            var page = pages.FirstOrDefault(x => x.Id == contextId);
            if (page is not null)
            {
                if (page.LabelInterpretation is null)
                    return string.Empty;

                return translationContext.TranslateLabel(
                    ActiveLanguage,
                    page.LabelInterpretation
                );
            }

            var match = FindContextById(contextId);
            if (match is FormControlContext control)
                return translationContext.TranslateLabel(ActiveLanguage, control.Interpretation);
            if (match is FormListContext list)
                return translationContext.TranslateLabel(ActiveLanguage, list.Interpretation);
            if (match is IFormElementContext context && context.Interpretation.Label is not null)
                return translationContext.TranslateLabel(ActiveLanguage, context.Interpretation.Label);

            throw new ArgumentException($"Could not get the translated label for context '{match.Id}'");
        }

        public FormPageContext GetPage(int index)
        {
            return pages[index];
        }

        public void InstantiateList(Guid listContextId)
        {
            var listMatch = FindContextById(listContextId);
            var listContext = CastList(listMatch);
            dataContext.InstantiateList(listContext);
        }

        public void ChangeLanguage(string language)
        {
            Validate();
            activeLanguage = language;
            notificationHandler.Notify(JsonFormNotificationType.OnLanguageChanged);
        }

        public void ChangeDisabled(bool disabled)
        {
            this.disabled = disabled;
            notificationHandler.Notify(JsonFormNotificationType.OnDisabledChanged);
        }

        public void ChangeReadOnly(bool readOnly)
        {
            this.readOnly = readOnly;
            notificationHandler.Notify(JsonFormNotificationType.OnReadOnlyChanged);
        }

        static FormControlContext CastControl(IFormElementContext context)
        {
            return context as FormControlContext
                ?? throw new InvalidCastException($"Context of type '{context.GetType()}' could not be cast to type '{typeof(FormControlContext)}'");
        }

        static FormListContext CastList(IFormElementContext context)
        {
            return context as FormListContext
                ?? throw new InvalidCastException($"Context of type '{context.GetType()}' could not be cast to type '{typeof(FormListContext)}'");
        }

        void EnforceRules()
        {
            var rootContexts = GetAllRootElementContexts();
            for (var j = 0; j < rootContexts.Length; j++)
            {
                var context = rootContexts[j];
                ruleEnforcer.EnforceRule(dataContext, context, rootContexts);
            }

            ruleEnforcer.EnforceRulesForPages(dataContext, pages, rootContexts);
        }

        IFormElementContext[] GetAllRootElementContexts()
        {
            var result = new List<IFormElementContext>();

            for (var i = 0; i < pages.Length; i++)
            {
                var page = pages[i];
                result.AddRange(page.ElementContexts);
            }

            return [.. result];
        }

        IFormElementContext FindContextById(Guid id)
        {
            foreach (var page in pages)
            {
                var result = page.FindContextById(id);
                if (result is not null)
                    return result;
            }

            throw new InvalidOperationException($"Could not find context by id '{id}'");
        }
    }
}
