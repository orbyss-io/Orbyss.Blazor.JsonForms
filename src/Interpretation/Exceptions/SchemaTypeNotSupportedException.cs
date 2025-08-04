namespace Orbyss.Blazor.JsonForms.Interpretation.Exceptions
{
    public sealed class SchemaTypeNotSupportedException : Exception
    {
        public SchemaTypeNotSupportedException(string? message = null) : base(message)
        {
        }
    }
}