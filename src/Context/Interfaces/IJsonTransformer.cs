using Newtonsoft.Json.Linq;

namespace Orbyss.Blazor.JsonForms.Context.Interfaces;

public interface IJsonTransformer
{
    JToken AddNewValue(string path, JToken data, JToken value);

    JToken SetValue(string path, JToken data, JToken value);

    JToken PutValue(string path, JToken data, JToken value);

    JToken AddValue(string path, JToken data, JToken value);

    JToken RemoveValue(string path, JToken data);
}