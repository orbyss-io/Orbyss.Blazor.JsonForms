namespace Orbyss.Components.JsonForms.Interpretation.Interfaces
{
    public interface IJsonPathInterpreter
    {
        string FromElementScope(string? scope);

        string JoinJsonPaths(string left, string right);

        string GetJsonPropertyNameFromPath(string path);

        string FromJsonSchemaPath(string path);

        string[] GetPathElements(string path);

        string AddIndexToPath(string path, int index);

        string GetParentPathFromSchemaPath(string schemaPath);

        string GetParentPathFromDataPath(string dataPath);
    }
}