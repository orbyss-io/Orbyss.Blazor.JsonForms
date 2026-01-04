using Orbyss.Blazor.JsonForms.Context.Interfaces;
using Orbyss.Blazor.JsonForms.Context.Models;
using Orbyss.Blazor.JsonForms.Context.Notifications;
using Orbyss.Blazor.JsonForms.Interpretation;
using Orbyss.Blazor.JsonForms.Interpretation.Interfaces;
using Orbyss.Blazor.JsonForms.Interpretation.Utils;

namespace Orbyss.Blazor.JsonForms.Context.Utils;

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
            dataContext ?? JsonFormDataContextBuilder.Build(jsonTransformer, jsonPathInterpreter, elementContextFactory),
            translationContext ?? new JsonFormTranslationContext(jsonPathInterpreter),
            uiSchemaInterpreter ?? UiSchemaInterpreterBuilder.Build(jsonPathInterpreter, controlTypeInterpreter),
            elementContextFactory ?? new FormElementContextFactory(jsonPathInterpreter),
            ruleEnforcer ?? new FormRuleEnforcer()
        );

        result.Instantiate(
            options
        );

        return result;
    }
}