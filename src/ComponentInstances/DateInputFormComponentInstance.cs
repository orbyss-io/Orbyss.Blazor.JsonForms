using Newtonsoft.Json.Linq;
using Orbyss.Components.Json.Models;

namespace Orbyss.Blazor.JsonForms.ComponentInstances
{
    public class DateTimeInputFormComponentInstance(Type componentGenericTypeDefinition) : DateInputFormComponentInstance<DateTime?>
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
    public class DateOnlyInputFormComponentInstance(Type componentGenericTypeDefinition) : DateInputFormComponentInstance<DateOnly?>
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

    public class DateTimeUtcTicksInputFormComponentInstance(Type componentGenericTypeDefinition) : DateInputFormComponentInstance<DateTimeUtcTicks?>
    {
        public override Type ComponentType => componentGenericTypeDefinition.MakeGenericType(typeof(DateTimeUtcTicks?));

        protected override sealed Func<DateTime?, DateTimeUtcTicks?>? ConvertFromDateTime => dt => dt.HasValue ? new DateTimeUtcTicks(dt.Value.Ticks) : null;

        protected override sealed Func<DateTimeUtcTicks?, DateTime?>? ConvertToDateTime => dt => dt.HasValue ? dt.Value.DateTime.DateTime : null;

        protected override sealed object? ConvertValue(JToken? token)
        {
            if (token is null)
            {
                return null;
            }

            return new DateTimeUtcTicks((long)token);
        }
    }


    public class DateUtcTicksInputFormComponentInstance(Type componentGenericTypeDefinition) : DateInputFormComponentInstance<DateUtcTicks?>
    {
        public override Type ComponentType => componentGenericTypeDefinition.MakeGenericType(typeof(DateUtcTicks?));

        protected override sealed Func<DateTime?, DateUtcTicks?>? ConvertFromDateTime => dt => dt.HasValue ? new DateUtcTicks(dt.Value.Ticks) : null;

        protected override sealed Func<DateUtcTicks?, DateTime?>? ConvertToDateTime => dt => dt.HasValue ? dt.Value.DateOnly.ToDateTime(TimeOnly.MinValue) : null;

        protected override sealed object? ConvertValue(JToken? token)
        {
            if (token is null)
            {
                return null;
            }

            return new DateUtcTicks((long)token);
        }
    }


    public abstract class DateInputFormComponentInstance<TDate> : InputFormComponentInstanceBase
    {                
        protected abstract Func<DateTime?, TDate?>? ConvertFromDateTime { get; }

        protected abstract Func<TDate?, DateTime?>? ConvertToDateTime { get; }

        public DateTime? MinimumDate { get; set; }

        public DateTime? MaximumDate { get; set; }

        public bool AllowManualInput { get; set; }

        protected override sealed IDictionary<string, object?> GetFormInputParameters()
        {
            var result = GetDateInputParameter();

            result[nameof(ConvertFromDateTime)] = ConvertFromDateTime;
            result[nameof(ConvertToDateTime)] = ConvertToDateTime;
            result[nameof(MinimumDate)] = MinimumDate;
            result[nameof(MaximumDate)] = MaximumDate;
            result[nameof(AllowManualInput)] = AllowManualInput;

            return result;
        }

        protected virtual IDictionary<string, object?> GetDateInputParameter()
        {
            return new Dictionary<string, object?>();
        }
    }
}
