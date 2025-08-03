
using Microsoft.AspNetCore.Components;

namespace Orbyss.Components.JsonForms.ComponentInstances
{
    public abstract class ListFormComponentInstanceBase : FormComponentInstanceBase
    {   
        public bool ReadOnly { get; internal set; }

        public bool Disabled { get; internal set; }

        public string? Title { get; internal set; }

        public string? Error { get; internal set; }

        public EventCallback OnAddItemClicked { get; internal set; }

        protected override sealed IDictionary<string, object?> GetParametersCore()
        {
            var result = GetListParameters();

            result[nameof(ReadOnly)] = ReadOnly;
            result[nameof(Disabled)] = Disabled;
            result[nameof(Title)] = Title;
            result[nameof(Error)] = Error;
            result[nameof(OnAddItemClicked)] = OnAddItemClicked;

            return result;
        }

        protected virtual IDictionary<string, object?> GetListParameters()
        {
            return new Dictionary<string, object?>();
        }
    }
}
