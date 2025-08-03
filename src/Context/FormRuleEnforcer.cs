using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Context.Models;
using Orbyss.Components.JsonForms.Interpretation;
using Orbyss.Components.JsonForms.UiSchema;

namespace Orbyss.Components.JsonForms.Context
{
    public sealed class FormRuleEnforcer : IFormRuleEnforcer
    {
        public void EnforceRule(IJsonFormDataContext dataContext, IFormElementContext context, IFormElementContext[] rootContexts)
        {
            if (context is FormVerticalLayoutContext verticalLayoutContext)
            {
                foreach (var row in verticalLayoutContext.Rows)
                {
                    EnforceRule(dataContext, row, rootContexts);
                }
            }
            else if (context is FormHorizontalLayoutContext horizontalLayoutContext)
            {
                foreach (var column in horizontalLayoutContext.Columns)
                {
                    EnforceRule(dataContext, column, rootContexts);
                }
            }
            else if (context is FormListContext listContext && listContext.Interpretation.Rule is not null)
            {
                EnforceRule(dataContext, listContext, listContext.Interpretation.Rule!, rootContexts);
            }
            else if (context is FormControlContext controlContext && controlContext.Interpretation.Rule is not null)
            {
                EnforceRule(dataContext, controlContext, controlContext.Interpretation.Rule!, rootContexts);
            }
        }

        public void EnforceRulesForPages(IJsonFormDataContext dataContext, FormPageContext[] pages, IFormElementContext[] rootContexts)
        {
            for (var i = 0; i < pages.Length; i++)
            {
                EnforceRuleForPage(dataContext, pages[i], rootContexts);
            }
        }

        static void EnforceRule(IJsonFormDataContext dataContext, IFormElementContext contextUnderEvaluation, UiSchemaRuleInterpretation rule, IFormElementContext[] rootContexts)
        {
            for (var i = 0; i < rootContexts.Length; i++)
            {
                var context = rootContexts[i];
                if (!context.FindDataPathBySchemaPath(rule.AbsoluteJsonSchemaPath, out var dataPath))
                {
                    continue;
                }

                var dataTokenToEvaluate = dataContext
                    .GetFormData()
                    .SelectToken(dataPath)
                    ?? JValue.CreateNull();

                if (dataTokenToEvaluate.IsValid(rule.Schema))
                {
                    switch (rule.Effect)
                    {
                        case UiSchemaElementRuleEffect.Hide:
                            contextUnderEvaluation.SetHidden(true);
                            break;
                        case UiSchemaElementRuleEffect.Show:
                            contextUnderEvaluation.SetHidden(false);
                            break;
                        case UiSchemaElementRuleEffect.Disable:
                            contextUnderEvaluation.SetDisabled(true);
                            break;
                        case UiSchemaElementRuleEffect.Enable:
                            contextUnderEvaluation.SetDisabled(false);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    contextUnderEvaluation.SetHidden(null);
                    contextUnderEvaluation.SetDisabled(null);
                }
            }
        }      

        static void EnforceRuleForPage(IJsonFormDataContext dataContext, FormPageContext pageContext, IFormElementContext[] rootContexts)
        {
            if (pageContext.Rule is null)
            {
                return;
            }

            for (var i = 0; i < rootContexts.Length; i++)
            {
                var context = rootContexts[i];
                if (!context.FindDataPathBySchemaPath(pageContext.Rule.AbsoluteJsonSchemaPath, out var dataPath))
                {
                    continue;
                }

                var dataTokenToEvaluate = dataContext
                    .GetFormData()
                    .SelectToken(dataPath)
                    ?? JValue.CreateNull();

                if (dataTokenToEvaluate.IsValid(pageContext.Rule.Schema))
                {
                    switch (pageContext.Rule.Effect)
                    {
                        case UiSchemaElementRuleEffect.Hide:
                            SetHiddenForPage(pageContext, true);
                            break;
                        case UiSchemaElementRuleEffect.Show:
                            SetHiddenForPage(pageContext, false);
                            break;
                        case UiSchemaElementRuleEffect.Disable:
                            SetDisabledForPage(pageContext, true);
                            break;
                        case UiSchemaElementRuleEffect.Enable:
                            SetDisabledForPage(pageContext, false);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    SetHiddenForPage(pageContext, null);
                    SetDisabledForPage(pageContext, null);
                }
            }
        }

        static void SetDisabledForPage(FormPageContext page, bool? value)
        {
            page.SetDisabled(value);
            for (var i = 0; i < page.ElementContexts.Length; i++)
            {
                SetDisabledForElement(page.ElementContexts[i], value);
            }
        }

        static void SetHiddenForPage(FormPageContext page, bool? value)
        {
            page.SetHidden(value);
            for (var i = 0; i < page.ElementContexts.Length; i++)
            {
                SetHiddenForElement(page.ElementContexts[i], value);
            }
        }

        static void SetDisabledForElement(IFormElementContext element, bool? value)
        {
            element.SetDisabled(value);
            if (element is FormVerticalLayoutContext verticalLayoutContext)
            {
                foreach (var row in verticalLayoutContext.Rows)
                {
                    SetDisabledForElement(row, value);
                }
            }
            else if (element is FormHorizontalLayoutContext horizontalLayoutContext)
            {
                foreach (var column in horizontalLayoutContext.Columns)
                {
                    SetDisabledForElement(column, value);
                }
            }
            else if (element is FormListContext listContext)
            {
                listContext.SetDisabled(value);
                foreach (var listItem in listContext.Items)
                {
                    listItem.SetDisabled(value);
                }
            }
            else if (element is FormControlContext controlContext)
            {
                controlContext.SetDisabled(value);
            }
        }

        static void SetHiddenForElement(IFormElementContext element, bool? value)
        {
            element.SetDisabled(value);
            if (element is FormVerticalLayoutContext verticalLayoutContext)
            {
                foreach (var row in verticalLayoutContext.Rows)
                {
                    SetHiddenForElement(row, value);
                }
            }
            else if (element is FormHorizontalLayoutContext horizontalLayoutContext)
            {
                foreach (var column in horizontalLayoutContext.Columns)
                {
                    SetHiddenForElement(column, value);
                }
            }
            else if (element is FormListContext listContext)
            {
                listContext.SetHidden(value);
                foreach (var listItem in listContext.Items)
                {
                    listItem.SetHidden(value);
                }
            }
            else if (element is FormListContext controlContext)
            {
                controlContext.SetHidden(value);
            }
        }        
    }
}
