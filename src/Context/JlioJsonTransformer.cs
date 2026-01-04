using JLio.Commands;
using JLio.Core;
using JLio.Core.Models;
using Newtonsoft.Json.Linq;
using Orbyss.Blazor.JsonForms.Context.Interfaces;

namespace Orbyss.Blazor.JsonForms.Context;

public sealed class JlioJsonTransformer : IJsonTransformer
{
    public JToken AddNewValue(string path, JToken data, JToken value)
    {
        var executeOptions = JLio.Core.Models.ExecutionContext.CreateDefault();
        var valueToAdd = new FunctionSupportedValue(new FixedValue(value));
        var result = new Add(path, valueToAdd)
            .Execute(JToken.Parse($"{data}"), executeOptions);

        if (!result.Success)
            throw new InvalidOperationException(string.Join(" | ", executeOptions.GetLogEntries().Select(y => y.Message)));

        return result.Data;
    }

    public JToken SetValue(string path, JToken data, JToken value)
    {
        var valueToSet = new FunctionSupportedValue(new FixedValue(value));
        var command = new Set(path, valueToSet);

        return ExecuteInternal(command, data);
    }

    public JToken RemoveValue(string path, JToken data)
    {
        var command = new Remove(path);

        return ExecuteInternal(command, data);
    }

    public JToken PutValue(string path, JToken data, JToken value)
    {
        var valueToPut = new FunctionSupportedValue(new FixedValue(value));
        var command = new Put(path, valueToPut);

        return ExecuteInternal(command, data);
    }

    public JToken AddValue(string path, JToken data, JToken value)
    {
        var valueToAdd = new FunctionSupportedValue(new FixedValue(value));
        var command = new Add(path, valueToAdd);

        return ExecuteInternal(command, data);
    }

    private static JToken ExecuteInternal(CommandBase command, JToken data)
    {
        var executeOptions = JLio.Core.Models.ExecutionContext.CreateDefault();
        var result = command.Execute(data, executeOptions);

        if (!result.Success)
            throw new InvalidOperationException(string.Join(" | ", executeOptions.GetLogEntries().Select(y => y.Message)));

        return result.Data;
    }
}