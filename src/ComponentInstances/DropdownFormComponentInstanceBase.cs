using Microsoft.AspNetCore.Components;
using Orbyss.Components.Json.Models;

namespace Orbyss.Components.JsonForms.ComponentInstances
{
    public abstract class DropdownFormComponentInstanceBase : InputFormComponentInstanceBase
    {
        public bool Clearable { get; set; }

        public bool Searchable { get; set; }

        public bool MultiSelect { get; internal set; }

        public IEnumerable<TranslatedEnumItem> Items { get; internal set; } = [];

        public EventCallback<IEnumerable<string>> OnSelectedValuesChanged { get; internal set; }

        public EventCallback<string> OnValueChanged { get; internal set; }

        protected override IDictionary<string, object?> GetFormInputParameters()
        {
            var result = GetDropdownParameters();

            AddIfNotContains(result, nameof(Clearable), Clearable);
            AddIfNotContains(result, nameof(Searchable), Searchable);

            result[nameof(MultiSelect)] = MultiSelect;
            result[nameof(Items)] = Items;
            result[nameof(OnValueChanged)] = OnValueChanged;
            result[nameof(OnSelectedValuesChanged)] = OnSelectedValuesChanged;

            return result;
        }

        protected virtual IDictionary<string, object?> GetDropdownParameters()
        {
            return new Dictionary<string, object?>();
        }
    }
}