using Orbyss.Blazor.JsonForms.Context.Models;
using Orbyss.Blazor.JsonForms.Interpretation;
using Orbyss.Blazor.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Blazor.JsonForms.Context.Interfaces;

public interface IFormElementContextFactory
{
    IFormElementContext Create(IUiSchemaElementInterpretation interpretation, string? parentAbsoluteDataJsonPath);

    FormPageContext[] CreatePages(UiSchemaPageInterpretation[] pageInterpretations);
}