using Orbyss.Components.JsonForms.ComponentInstances;
using Orbyss.Components.JsonForms.ComponentInstances.Interfaces;
using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Context.Models;

namespace Orbyss.Components.JsonForms
{
    public interface IFormComponentInstanceProvider
    {
        InputFormComponentInstanceBase GetInputField(IJsonFormContext context, FormControlContext control);

        IFormComponentInstance GetGridRow(IFormElementContext? row);

        IFormComponentInstance GetGridColumn(IFormElementContext? column);

        IFormComponentInstance GetGrid(IJsonFormContext? form, FormPageContext? page);

        ButtonFormComponentInstanceBase GetButton(FormButtonType type, IJsonFormContext? form);

        NavigationFormComponentInstanceBase GetNavigation(IJsonFormContext formContext);

        ListFormComponentInstanceBase GetList(FormListContext? list = null);

        ListItemFormComponentInstance GetListItem(IFormElementContext? listItem = null);
    }

    public enum FormButtonType
    {
        Submit,
        Next,
        Previous
    }
}
