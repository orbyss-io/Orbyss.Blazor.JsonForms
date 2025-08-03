using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Context.Models;
using Orbyss.Components.JsonForms.Context.Notifications;
using Orbyss.Components.JsonForms.Interpretation;
using Orbyss.Components.JsonForms.Interpretation.Interfaces;
using Orbyss.Components.JsonForms.Interpretation.Utils;

namespace Orbyss.Components.JsonForms.Context.Utils
{
    public static class JsonFormContextBuilder
    {
        public static IJsonFormContext BuildAndInstantiate(
            JsonFormContextInitOptions options,
            IJsonFormDataContext? dataContext = null,
            IJsonFormTranslationContext? translationContext = null,
            IJsonFormNotificationHandler? notificationHandler = null,
            IFormUiSchemaInterpreter? uiSchemaInterpreter = null,
            IFormElementContextFactory? elementContextFactory = null,
            IFormRuleEnforcer? ruleEnforcer = null,
            IJsonTransformer? jsonTransformer = null,
            IJsonPathInterpreter? jsonPathInterpreter = null,
            IControlTypeInterpreter? controlTypeInterpreter = null
        )
        {
            jsonPathInterpreter ??= JsonPathInterpreter.Default;

            var result = new JsonFormContext(
                notificationHandler ?? new JsonFormNotificationHandler(),
                uiSchemaInterpreter ?? UiSchemaInterpreterBuilder.Build(jsonPathInterpreter, controlTypeInterpreter),
                elementContextFactory ?? new FormElementContextFactory(jsonPathInterpreter),
                dataContext ?? JsonFormDataContextBuilder.Build(jsonTransformer, jsonPathInterpreter, elementContextFactory),
                translationContext ?? new JsonFormTranslationContext(jsonPathInterpreter),
                ruleEnforcer ?? new FormRuleEnforcer()
            );

            result.Instantiate(
                options
            );

            return result;
        }
    }
}