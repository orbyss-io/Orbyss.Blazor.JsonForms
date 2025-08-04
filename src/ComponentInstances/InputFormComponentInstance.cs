using Newtonsoft.Json.Linq;

namespace Orbyss.Blazor.JsonForms.ComponentInstances
{
    public class InputFormComponentInstance(Type componentType, Func<JToken?, object?> convertValue) : InputFormComponentInstanceBase
    {
        public override sealed Type ComponentType { get; } = componentType;

        protected override sealed object? ConvertValue(JToken? value)
        {
            return convertValue(value);
        }
    }

    public class InputFormComponentInstance<TComponent>(Func<JToken?, object?> convertValue)

        : InputFormComponentInstance(typeof(TComponent), convertValue)

        where TComponent : class
    {
    }
}