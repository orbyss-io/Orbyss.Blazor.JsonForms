using Orbyss.Blazor.JsonForms.Context.Models;

namespace Orbyss.Blazor.JsonForms.Context.Interfaces;

public interface IFormRuleEnforcer
{
    void EnforceRule(IJsonFormDataContext dataContext, IFormElementContext context, IFormElementContext[] rootContexts);

    void EnforceRulesForPages(IJsonFormDataContext dataContext, FormPageContext[] pages, IFormElementContext[] rootContexts);
}