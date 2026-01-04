using Newtonsoft.Json.Linq;

namespace Orbyss.Blazor.JsonForms.ComponentInstances;

public class DateOnlyInputFormComponentInstance(Type componentGenericTypeDefinition) : DateOnlyInputFormComponentInstanceBase<DateOnly?>
{
    public override Type ComponentType => componentGenericTypeDefinition.MakeGenericType(typeof(DateOnly?));

    protected override sealed Func<DateTime?, DateOnly?>? ConvertFromDateTime => dt => dt.HasValue ? new DateOnly(dt.Value.Year, dt.Value.Month, dt.Value.Day) : null;

    protected override sealed Func<DateOnly?, DateTime?>? ConvertToDateTime => dt => dt.HasValue ? dt.Value.ToDateTime(TimeOnly.MinValue) : null;

    protected override sealed object? ConvertValue(JToken? token)
    {
        return DateOnly.TryParse($"{token}", Culture, out var date)
            ? date
            : null;
    }
}
