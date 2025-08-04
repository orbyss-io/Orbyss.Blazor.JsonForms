namespace Orbyss.Blazor.JsonForms.Interpretation.Exceptions
{
    public sealed class InvalidSchemaTypeConfigurationException : Exception
    {
        public InvalidSchemaTypeConfigurationException(string? message = null) : base(message)
        {
        }
    }
}