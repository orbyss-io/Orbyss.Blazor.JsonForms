using Newtonsoft.Json.Linq;
using Orbyss.Components.Json.Models;

namespace Orbyss.Blazor.JsonForms.ComponentInstances;


public class DateTimeUtcTicksInputFormComponentInstance(Type componentGenericTypeDefinition) : DateTimeInputFormComponentInstanceBase<DateTimeUtcTicks?>
{
    public override Type ComponentType => componentGenericTypeDefinition.MakeGenericType(typeof(DateTimeUtcTicks?));

    protected override sealed Func<DateTime?, DateTimeUtcTicks?>? ConvertFromDateTime => dt => dt.HasValue ? new DateTimeUtcTicks(dt.Value.Ticks) : null;

    protected override sealed Func<DateTimeUtcTicks?, DateTime?>? ConvertToDateTime => dt => dt.HasValue ? dt.Value.DateTime.DateTime : null;

    protected override sealed object? ConvertValue(JToken? token)
    {
        if (token is null || string.IsNullOrWhiteSpace($"{token}"))
        {
            return null;
        }

        return new DateTimeUtcTicks((long)token);
    }
}
