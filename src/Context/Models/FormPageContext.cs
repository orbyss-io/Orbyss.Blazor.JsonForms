using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Interpretation;

namespace Orbyss.Components.JsonForms.Context.Models
{
    public sealed class FormPageContext(
        UiSchemaPageInterpretation pageInterpretation,
        IFormElementContext[] elementContexts)
    {
        private readonly Guid id = Guid.NewGuid();
        private bool? disabledOverwrite;
        private bool? hiddenOverwrite;

        public IFormElementContext[] ElementContexts { get; } = elementContexts;

        public UiSchemaLabelInterpretation? LabelInterpretation { get; } = pageInterpretation.LabelInterpretation;

        public Guid Id => id;

        public IFormElementContext? FindContextById(Guid id)
        {
            return FindInElements(ElementContexts, id);
        }

        public bool Disabled => disabledOverwrite ?? pageInterpretation.Disabled;

        public bool Hidden => hiddenOverwrite ?? pageInterpretation.Hidden;

        public UiSchemaRuleInterpretation? Rule { get; } = pageInterpretation.Rule;

        public void SetHidden(bool? value)
        {
            hiddenOverwrite = value;
        }

        public void SetDisabled(bool? value)
        {
            disabledOverwrite = value;
        }

        private IFormElementContext? FindByIdInternal(IFormElementContext elementContext, Guid id)
        {
            return elementContext.Interpretation.ElementType switch
            {
                UiSchemaElementInterpretationType.VerticalLayout => FindInVerticalLayout((FormVerticalLayoutContext)elementContext, id),
                UiSchemaElementInterpretationType.HorizontalLayout => FindInHorizontalLayout((FormHorizontalLayoutContext)elementContext, id),
                UiSchemaElementInterpretationType.List => FindInList((FormListContext)elementContext, id),
                UiSchemaElementInterpretationType.Control => elementContext.Id == id ? elementContext : null,

                _ => throw new NotSupportedException($"Element type '{elementContext.Interpretation.ElementType} is not supported'")
            };
        }

        private IFormElementContext? FindInList(FormListContext listContext, Guid id)
        {
            if (listContext.Id == id)
                return listContext;

            return FindInElements(listContext.Items, id);
        }

        private IFormElementContext? FindInVerticalLayout(FormVerticalLayoutContext verticalLayoutContext, Guid id)
        {
            if (verticalLayoutContext.Id == id)
                return verticalLayoutContext;

            return FindInElements(verticalLayoutContext.Rows, id);
        }

        private IFormElementContext? FindInHorizontalLayout(FormHorizontalLayoutContext horizontalLayoutContext, Guid id)
        {
            if (horizontalLayoutContext.Id == id)
                return horizontalLayoutContext;

            return FindInElements(horizontalLayoutContext.Columns, id);
        }

        private IFormElementContext? FindInElements(IEnumerable<IFormElementContext> elementContexts, Guid id)
        {
            foreach (var context in elementContexts)
            {
                var result = FindByIdInternal(context, id);
                if (result is not null)
                    return result;
            }

            return null;
        }
    }
}
