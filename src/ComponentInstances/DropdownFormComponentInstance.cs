using Newtonsoft.Json.Linq;

namespace Orbyss.Components.JsonForms.ComponentInstances
{
    public class DropdownFormComponentInstance(Type componentType) : DropdownFormComponentInstanceBase
    {
        public override Type ComponentType => componentType;

        protected override sealed object? ConvertValue(JToken? value)
        {
            if (MultiSelect)
            {
                return value?.ToObject<IEnumerable<string>>();
            }

            return value?.ToString();
        }
    }

    public class DropdownFormComponentInstance<TComponent> : DropdownFormComponentInstance
    {
        public DropdownFormComponentInstance() : base(typeof(TComponent))
        {
        }
    }
}