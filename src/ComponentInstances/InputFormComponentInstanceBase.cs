using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;

namespace Orbyss.Components.JsonForms.ComponentInstances
{
    public abstract class InputFormComponentInstanceBase : FormComponentInstanceBase
    {
        JToken? value;

        public string? Label { get; internal set; }        

        public bool Disabled { get; internal set; }

        public bool ReadOnly { get; internal set; }

        public string? ErrorHelperText { get; internal set; }

        public string? HelperText { get; set; }

        public object? Value => ConvertValue(value);

        internal void SetInputValue(JToken? value)
        {
            this.value = value;
        }

        protected abstract object? ConvertValue(JToken? value);

        protected virtual IDictionary<string, object?> GetFormInputParameters()
        {
            return new Dictionary<string, object?>();
        }

        protected override sealed IDictionary<string, object?> GetParametersCore()
        {
            var result = GetFormInputParameters();
            AddIfNotContains(result, nameof(HelperText), HelperText);

            result[nameof(Label)] = Label;
            result[nameof(Disabled)]= Disabled;
            result[nameof(ReadOnly)] = ReadOnly;
            result[nameof(ErrorHelperText)] =ErrorHelperText;
            result[nameof(Value)] = Value;

            return result;
        }
    }
}
