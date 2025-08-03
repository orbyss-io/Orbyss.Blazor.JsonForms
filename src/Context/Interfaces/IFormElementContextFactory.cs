using Orbyss.Components.JsonForms.Context.Models;
using Orbyss.Components.JsonForms.Interpretation;
using Orbyss.Components.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Components.JsonForms.Context.Interfaces
{
    public interface IFormElementContextFactory
    {
        IFormElementContext Create(IUiSchemaElementInterpretation interpretation, string? parentAbsoluteDataJsonPath);

        FormPageContext[] CreatePages(UiSchemaPageInterpretation[] pageInterpretations);
    }
}