namespace Orbyss.Blazor.JsonForms.Interpretation.Exceptions
{
    public sealed class SchemaTypeNotSpecifiedException : Exception
    {
        public SchemaTypeNotSpecifiedException(string? message = null) : base(message)
        {
        }
    }
}