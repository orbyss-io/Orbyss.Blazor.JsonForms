using Newtonsoft.Json.Linq;
using Orbyss.Components.Json.Models;

namespace Orbyss.Blazor.JsonForms.ComponentInstances;

public class DateOnlyUtcTicksInputFormComponentInstance(Type componentGenericTypeDefinition) : DateOnlyInputFormComponentInstanceBase<DateUtcTicks?>
{
    public override Type ComponentType => componentGenericTypeDefinition.MakeGenericType(typeof(DateUtcTicks?));

    protected override sealed Func<DateTime?, DateUtcTicks?>? ConvertFromDateTime => dt => dt.HasValue ? new DateUtcTicks(dt.Value.Ticks) : null;

    protected override sealed Func<DateUtcTicks?, DateTime?>? ConvertToDateTime => dt => dt.HasValue ? dt.Value.DateOnly.ToDateTime(TimeOnly.MinValue) : null;

    protected override sealed object? ConvertValue(JToken? token)
    {
        if (token is null || string.IsNullOrWhiteSpace($"{token}"))
        {
            return null;
        }

        return new DateUtcTicks((long)token);
    }
}
