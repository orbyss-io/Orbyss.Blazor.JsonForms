namespace Orbyss.Blazor.JsonForms.ComponentInstances;

public abstract class DateOnlyInputFormComponentInstanceBase<TDate> : InputFormComponentInstanceBase
{
    protected abstract Func<DateTime?, TDate>? ConvertFromDateTime { get; }

    protected abstract Func<TDate, DateTime?>? ConvertToDateTime { get; }

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
