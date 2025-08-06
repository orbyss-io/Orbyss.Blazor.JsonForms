# 📦 Orbyss.Blazor.JsonForms

**A fully .NET-native implementation of the [JsonForms.io](https://jsonforms.io) standard for schema-driven forms in Blazor.**  
No Angular, no web components — just C#, JSON Schema, and flexible Blazor architecture.
![NuGet](https://img.shields.io/nuget/v/Orbyss.Blazor.JsonForms)
![NuGet Downloads](https://img.shields.io/nuget/dt/Orbyss.Blazor.JsonForms)
---

## 🎯 What is this?

This is the **UI-agnostic core framework** for rendering dynamic forms from JSON Schema in .NET. It handles:

- Form generation
- Schema interpretation
- Localization via translation schema
- Layout, validation, and data management

You plug in the **UI layer**, also called the 'FormComponentInstanceProvider'. This library is the form engine — and you must bring the renderer.

With an implemented UI layer, you can render the JsonForms in your code as follows:

```csharp
<JsonForm InitOptions=@options/>

@code{
    JsonFormContextInitOptions options = new(
        jsonSchema,
        uiSchema,
        translationSchema
    );
}
```
> ❗You can specify JsonFormContext as parameter, or as a **Transient Service** (DI)
>
> ❗You can specify ComponentInstanceProvider as parameter, or as a DI service
>
> ❗You can provide the following cascading values to your JsonForm: "Language, Disabled, ReadOnly".


---

## 🚀 Available UI Integrations

Use one of our ready-to-go UI packages:

- 🧩 [Orbyss.Blazor.Syncfusion.JsonForms](https://www.nuget.org/packages/Orbyss.Blazor.Syncfusion.JsonForms)
- 🎨 [Orbyss.Blazor.MudBlazor.JsonForms](https://www.nuget.org/packages/Orbyss.Blazor.MudBlazor.JsonForms)

Or you can build your own; for example when you have your own Blazor component system, or you are using other external frameworks such as **Radzen**, **Telerik**, or **Fluent UI**

---

## 🛠 How to: Implement Your Own UI Layer

### ✅ Start by implementing the following interface

```csharp
public interface IFormComponentInstanceProvider
{
    InputFormComponentInstanceBase GetInputField(IJsonFormContext context, FormControlContext control);
    IFormComponentInstance GetGridRow(IFormElementContext? row);
    IFormComponentInstance GetGridColumn(IFormElementContext? column);
    IFormComponentInstance GetGrid(IJsonFormContext? form, FormPageContext? page);
    ButtonFormComponentInstanceBase GetButton(FormButtonType type, IJsonFormContext? form);
    NavigationFormComponentInstanceBase GetNavigation(IJsonFormContext formContext);
    ListFormComponentInstanceBase GetList(FormListContext? list = null);
    ListItemFormComponentInstance GetListItem(IFormElementContext? listItem = null);
}
```

Example of a GetInputField implementation:
```csharp
 public virtual InputFormComponentInstanceBase GetInputField(IJsonFormContext context, FormControlContext control)
 {
     var type = control.Interpretation.ControlType;

     return type switch
     {
         ControlType.Boolean => GetBooleanField(control),
         ControlType.String => GetTextField(control),
         ControlType.Enum => GetDropDownField(control),
         ControlType.EnumList => GetMultiDropDownField(control),
         ControlType.DateTime => GetDateTimeField(control),
         ControlType.DateOnly => GetDateOnlyField(control),
         ControlType.DateOnlyUtcTicks => GetDateUtcTicksField(control),
         ControlType.DateTimeUtcTicks => GetDateTimeUtcTicksField(control),

         ControlType.Integer => GetIntegerField(control),
         ControlType.Number => GetNumberField(control),

         _=> throw new NotSupportedException($"Cannot create an input field for type '{type}'")
     };
 }
```

---

### 🧱 Step-by-Step Guide to Building a Custom Component
#### ✅ 1. Create your Razor input component

```cshtml
<SfTextBox Type=InputType.Number
           CssClass="@FullClass"
           Enabled=@(!Disabled)
           Readonly=ReadOnly
           Value="@Value"
           Placeholder="@Label"
           FloatLabelType="FloatLabelType.Always"
           ID="@id"
           ValueChanged="@ValueChangedHandler"
           ShowClearButton=@Clearable
           Width="@Width" />


@if (HasError)
{
    <div class="e-error validation-message">@ErrorHelperText</div>
}
else if (!string.IsNullOrWhiteSpace(HelperText))
{
    <div class="validation-message "><i>@HelperText</i></div>
}
```

Add these standard parameters:
```csharp
[Parameter] public string? Label { get; set; } // Internally set
[Parameter] public bool Disabled { get;  set; } // Internally set
[Parameter] public bool ReadOnly { get; set; } // Internally set
[Parameter] public string? ErrorHelperText { get; set; } // Internally set

[Parameter] public string Value { get; set; } // Required: runtime error thrown when not specified
[Parameter] public EventCallback<string> ValueChanged { get; set; } // Required: runtime error thrown when not specified
```
> ⚠️ If you forget to invoke ValueChanged, your input won’t update the form state!
> 
> ⚠️ The control types are fixed. You must define the correctly typed parameters T Value & ValueChanged<T> in your components based on the control your component is for. See the lookup below.

```csharp
public static class ControlTypeLookup
{
    public static readonly Type Enum = typeof(string);
    public static readonly Type EnumList = typeof(IEnumerable<string>);
    public static readonly Type DateTime = typeof(DateTime?);
    public static readonly Type DateTimeUtcTicks = typeof(DateTimeUtcTicks?);
    public static readonly Type DateOnly = typeof(DateOnly?);
    public static readonly Type DateOnlyUtcTicks = typeof(DateUtcTicks?);
    public static readonly Type Number = typeof(double?);
    public static readonly Type Integer = typeof(int?);
    public static readonly Type String = typeof(string);
    public static readonly Type Boolean = typeof(bool);

    public static Type GetForControlType(ControlType controlType) => fieldsPerControlType[controlType];
}
```

Example: If your schema field is "type": "integer", your component must have:
```csharp
[Parameter] public int? Value { get; set; }
[Parameter] public EventCallback<int?> ValueChanged { get; set; }
```

#### ✅ 2. Create a component instance class
This class represents the contract of your component, and is used internally to map component parameters from the IFormComponentInstanceProvider implementation to your razor components.
When the standard parameters are sufficient for your components, you can simply make use of our built-in instances:

```csharp
 public virtual ListItemFormComponentInstance GetListItem(IFormElementContext? listItem = null)
 {
     return new ListItemFormComponentInstance<SyncfusionFormListItem>();
 }
```

If the built-in component instances do not provide all the parameters you need for your own components, you will have to derive from one of the (base)classes and add your own parameters.

**Custom razor component:**
```cshtml
<!-- MyCustomSwitch.razor -->
<SfSwitch HtmlAttributes=@htmlAttributes
          Checked="@Value"
          CssClass="@Class"
          TChecked="bool"
          CheckedChanged="OnValueChanged"
          Disabled=@(Disabled || ReadOnly)
          OffLabel="@OffLabel"
          OnLabel="@OnLabel" />

<!-- Custom Parameters -->
 [Parameter]
 public string? OnLabel { get; set; }

 [Parameter]
 public string? OffLabel { get; set; }
```

**Custom component instance:**
```csharp
public class MyCustomSwitchInstance : InputFormComponentInstance<MyCustomSwitch>
{
    public MyCustomSwitchInstance() : base(t => (bool?)t)
    {            
    }
    
    public string? OnLabel { get; set; }

    public string? OffLabel { get; set; }

    protected override IDictionary<string, object?> GetFormInputParameters()
    {
        return new Dictionary<string, object?>
        {
            [nameof(MyCustomSwitch.OffLabel)] = OffLabel,
            [nameof(MyCustomSwitch.OnLabel)] = OnLabel
        };
    }
}
```

#### ✅ 3. Return your instance

```csharp
  protected virtual InputFormComponentInstanceBase GetBooleanField(FormControlContext control)
  {
    var booleanControlType = control.Interpretation.GetOption("custom-bool-type");
    if($"{booleanControlType}" == "switch")
    {
        return new MyCustomSwitchInstance();
    }
    else
    {
        return new SyncfusionCheckboxInstance();
    }
  }
```

> 💡 You can make use of (custom) options that you can configure in your UI Schema. See [jsonforms.io docs](https://jsonforms.io/docs/uischema/controls#options)


---

## 🔄 How The Framework Works

The framework generates forms using three different schemas:

| Schema Type            | Description                                     |
| ---------------------- | ----------------------------------------------- |
| **JSON Schema**        | Defines the data structure (types, enums, etc.) |
| **UI Schema**          | Controls layout, grouping, (custom) options, [rules](https://jsonforms.io/docs/uischema/rules/)   |
| **Translation Schema** | Provides localized labels, errors, and enums |


Example:
```json
// JSON Schema
{
    "type": "object",
    "properties": {
        "firstName": {
            "type": "string",
            "minLength": 21
        },
        "surname": {
            "type": "string"
        }
    },
    "required":[
        "firstName"
    ]
}

// UI Schema
{
    "type": "VerticalLayout",
    "elements": [
        {
            "type": "Control",
            "scope": "#/properties/firstName"
        },
         {
            "type": "Control",
            "scope": "#/properties/surname",
            "options": {
                "hidden": true
            },
            "rule":{
                "effect": "Show",
                "condition":{
                    "scope": "#/properties/firstName",
                    "schema":{
                        "minLength": 2
                    }
                }
            }
        }
    ],
    "options": {
        "customOption": "custom-option-value"
    }
}

// Translation Schema
{
    "resources": {
        "en": {
            "translation": {
                "firstName": {
                    "label": "First Name",
                    "error":{
                        "minLength": "Must have minimum of 21 characters"
                    }
                },
                "surname": {
                    "label": "Surname"
                },
                "customLabel": "Special"
            }
        },
        "nl": {
            "translation": {
                "firstName": {
                    "label": "Voornaam",
                    "error":{
                        "minLength": "Moet minimaal 21 karakters bevatten"
                    }
                },
                "surname": {
                    "label": "Achternaam"
                },
                "customLabel": "Speciaal"
            }
        }
    }
}
```

---

## 📦 Installation
```bash
dotnet add package Orbyss.Components.JsonForms
```

Then reference a UI implementation package or build your own.

## 📄 License
MIT License
© Orbyss

## 🔗 Links
- 🌍 **Website**: [https://orbyss.io](https://orbyss.io)
- 📦 **NuGet**: [Orbyss.Blazor.JsonForms](https://www.nuget.org/packages/Orbyss.Blazor.JsonForms)
- 🧑‍💻 **GitHub**: [https://github.com/Orbyss-io](https://github.com/orbyss-io)
- 📝 **License**: [MIT](./LICENSE)
- [JsonForms.io](https://jsonforms.io/)
- [Syncfusion UI integration](https://www.nuget.org/packages/Orbyss.Blazor.Syncfusion.JsonForms)
- [MudBlazor UI integration](https://www.nuget.org/packages/Orbyss.Blazor.MudBlazor.JsonForms)

## 🤝 Contributing

This project is open source and contributions are welcome!

Whether it's bug fixes, improvements, documentation, or ideas — we encourage developers to get involved.  
Just fork the repo, create a branch, and open a pull request.

We follow standard .NET open-source conventions:
- Write clean, readable code
- Keep PRs focused and descriptive
- Open issues for larger features or discussions

No formal contribution guidelines — just be constructive and respectful.

---


⭐️ If you find this useful, [give us a star](https://github.com/orbyss-io/Orbyss.Components.Json.Models/stargazers) and help spread the word!
