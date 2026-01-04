using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Blazor.JsonForms.Context.Interfaces;
using Orbyss.Blazor.JsonForms.Context.Models;
using Orbyss.Blazor.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Blazor.JsonForms.Context;

public sealed class JsonFormDataContext(
    IJsonTransformer jsonTransformer,
    IFormElementContextFactory elementContextFactory,
    IJsonPathInterpreter jsonPathInterpreter)

    : IJsonFormDataContext
{
    private JToken? data;
    private JToken? dataSchema;

    public void Instantiate(JToken formData, JSchema dataSchema)
    {
        if (data is not null)
            throw new InvalidOperationException("Data context is already instantiated");

        data = JToken.Parse($"{formData}");
        this.dataSchema = JToken.Parse($"{dataSchema}");
    }

    public JSchema GetJsonSchema()
    {
        return JSchema.Parse($"{dataSchema}");
    }

    public bool Validate(IEnumerable<IFormElementContext> contexts)
    {
        var formData = JToken.Parse($"{data}");
        var schema = JToken.Parse($"{dataSchema}");

        var isvalid = true;

        foreach (var context in contexts)
        {
            if (!context.Validate(formData, schema))
            {
                isvalid = false;
            }
        }

        return isvalid;
    }

    public JToken? GetValue(FormControlContext formControlContext)
    {
        return GetFormData().SelectToken(formControlContext.AbsoluteDataJsonPath, false);
    }

    public void UpdateValue(FormControlContext formControlContext, JToken? value)
    {
        jsonTransformer.PutValue(formControlContext.AbsoluteDataJsonPath, GetFormData(), value ?? JValue.CreateNull());
    }

    public JToken GetFormData()
    {
        if (data is null)
            throw new InvalidOperationException("Form data is null");

        return data;
    }

    public void AddListItem(FormListContext listContext)
    {
        var listElementInterpretation = listContext.Interpretation;
        var newItemIndex = listContext.Items.Length;
        var newItemAbsolutePath = jsonPathInterpreter.AddIndexToPath(listContext.AbsoluteDataJsonPath, newItemIndex);
        var detail = elementContextFactory.Create(listElementInterpretation.GetListDetail(), newItemAbsolutePath);
        listContext.AddItem(detail);

        var itemExists = GetFormData().SelectToken(newItemAbsolutePath, false) is not null;
        if (!itemExists)
        {
            jsonTransformer.AddValue(listContext.AbsoluteDataJsonPath, GetFormData(), new JObject());
        }
    }

    public void RemoveListItem(FormListContext listContext, IFormElementContext listItemContext)
    {
        var removedItemIndex = listContext.RemoveItem(listItemContext.Id);
        var removedItemAbsolutePath = jsonPathInterpreter.AddIndexToPath(listContext.AbsoluteDataJsonPath, removedItemIndex);

        jsonTransformer.RemoveValue(removedItemAbsolutePath, GetFormData());
    }

    public void InstantiateList(FormListContext listContext)
    {
        var formData = GetFormData();
        var listData = formData.SelectToken(listContext.AbsoluteDataJsonPath, false);
        if (listData is null)
        {
            jsonTransformer.AddValue(listContext.AbsoluteDataJsonPath, formData, new JArray());
            listData = formData.SelectToken(listContext.AbsoluteDataJsonPath, true);
        }
        else if (listData is not JArray)
        {
            throw new InvalidOperationException($"Expected a JSON array at path '{listContext.AbsoluteDataJsonPath}', but instead found a '{listData.GetType().Name}'");
        }

        var list = (JArray)listData!;
        while (listContext.Items.Length < list.Count)
        {
            AddListItem(listContext);
        }

        if (listContext.Items.Length != list.Count)
        {
            throw new InvalidOperationException("List context contains more items than in the data while being instantiated");
        }
    }
}