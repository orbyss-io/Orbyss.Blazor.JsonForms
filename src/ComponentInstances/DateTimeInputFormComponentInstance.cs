using Newtonsoft.Json.Linq;

namespace Orbyss.Blazor.JsonForms.ComponentInstances
{
    public class DateTimeInputFormComponentInstance(Type componentGenericTypeDefinition) : DateTimeInputFormComponentInstanceBase<DateTime?>
    {
        public override Type ComponentType => componentGenericTypeDefinition.MakeGenericType(typeof(DateTime?));

        protected override sealed Func<DateTime?, DateTime?>? ConvertFromDateTime => (dt) => dt;

        protected override sealed Func<DateTime?, DateTime?>? ConvertToDateTime => (dt) => dt;

        protected override sealed object? ConvertValue(JToken? token)
        {
            return DateTime.TryParse($"{token}", Culture, out var dateTime)
                ? dateTime
                : null;
        }
    }     
}
