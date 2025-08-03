namespace Orbyss.Components.JsonForms.Context.Translations
{
    public static class DefaultJsonFormValidationMessages
    {
        public static string Default { get; set; } = "Input is invalid";
        public static string Minimum { get; set; } = "Input is greater than minimum required value";
        public static string Maximum { get; set; } = "Input is less than maximum allowed value";
        public static string MaxItems { get; set; } = "List has more than the maximum allowed items";
        public static string MinItems { get; set; } = "List has less than the minimum required items";
        public static string MaxLength { get; set; } = "Input has more than maximum allowed characters";
        public static string MinLength { get; set; } = "Input has less than the minimum required characters";
        public static string Contains { get; set; } = "Input does not contain all required values";
        public static string Required { get; set; } = "Field is required";
        public static string Pattern { get; set; } = "Input does not match regex pattern";
        public static string Const { get; set; } = "Input is not equal to the constant value";
    }
}
