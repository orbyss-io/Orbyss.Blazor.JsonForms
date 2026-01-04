using System.Globalization;

namespace Orbyss.Blazor.JsonForms.Constants;

public static class FormCulture
{
    public static CultureInfo Instance { get; set; } = new CultureInfo("en-US");
}
