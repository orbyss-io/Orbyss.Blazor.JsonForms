using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Blazor.JsonForms.Context;
using Orbyss.Blazor.JsonForms.Context.Utils;
using Orbyss.Blazor.JsonForms.Interpretation;
using Orbyss.Blazor.JsonForms.UiSchema;

namespace Orbyss.Blazor.JsonForms.Tests.Context;

[TestFixture]
public sealed class FormRuleEnforcerTests
{
    private const string jsonSchema = "{\"properties\":{\"address\":{\"type\":\"object\",\"properties\":{\"street\":{\"type\":\"string\"},\"streetNumber\":{\"type\":\"string\"}}}}}";
    private static readonly FormElementContextFactory contextFactory = new(new JsonPathInterpreter());

    [Test]
    [TestCase(UiSchemaElementRuleEffect.Hide, true)]
    [TestCase(UiSchemaElementRuleEffect.Show, false)]
    public void When_EnforceHideAndShowRules_And_RuleMustBeEnforced_Then_Applies_Effect_To_ContextUnderEvaluation(UiSchemaElementRuleEffect effect, bool expectedHiddenValue)
    {
        // Arrange
        var initialHiddenValue = !expectedHiddenValue;

        var formData = new JObject
        {
            ["address"] = new JObject
            {
                ["streetNumber"] = "12"
            }
        };
        var dataContext = JsonFormDataContextBuilder.BuildAndInstantiate(
            JSchema.Parse(jsonSchema),
            formData
        );

        var rule = new UiSchemaRuleInterpretation(
            "$.properties.address.properties.streetNumber",
            JSchema.Parse("{\"const\": \"12\"}"),
            effect
        );
        var streetInterpretation = CreateControlInterpretation(
            "$.properties.address.properties.street",
            "$.properties.street",
            "$.properties.address",
            rule,
            initialHiddenValue: initialHiddenValue
        );
        var streetContext = contextFactory.Create(streetInterpretation, "$.address");

        var streetNumberInterpretation = CreateControlInterpretation(
            "$.properties.address.properties.streetNumber",
            "$.properties.streetNumber",
            "$.properties.address",
            rule: null
        );
        var streetNumberContext = contextFactory.Create(streetNumberInterpretation, "$.address");

        var sut = new FormRuleEnforcer();

        // Pre-Assert
        Assert.That(streetContext.Hidden, Is.EqualTo(initialHiddenValue));

        // Act
        sut.EnforceRule(dataContext, streetContext, [streetContext, streetNumberContext]);

        // Assert
        Assert.That(streetContext.Hidden, Is.EqualTo(expectedHiddenValue));
    }

    [Test]
    [TestCase(UiSchemaElementRuleEffect.Disable, true)]
    [TestCase(UiSchemaElementRuleEffect.Enable, false)]
    public void When_EnforceDisableAndEnableRules_And_RuleMustBeEnforced_Then_Applies_Effect_To_ContextUnderEvaluation(UiSchemaElementRuleEffect effect, bool expectedDisabledValue)
    {
        // Arrange
        var initialDisabledValue = !expectedDisabledValue;

        var formData = new JObject
        {
            ["address"] = new JObject
            {
                ["streetNumber"] = "12"
            }
        };
        var dataContext = JsonFormDataContextBuilder.BuildAndInstantiate(
            JSchema.Parse(jsonSchema),
            formData
        );

        var rule = new UiSchemaRuleInterpretation(
            "$.properties.address.properties.streetNumber",
            JSchema.Parse("{\"const\": \"12\"}"),
            effect
        );
        var streetInterpretation = CreateControlInterpretation(
            "$.properties.address.properties.street",
            "$.properties.street",
            "$.properties.address",
            rule,
            initialDisabledValue: initialDisabledValue
        );
        var streetContext = contextFactory.Create(streetInterpretation, "$.address");

        var streetNumberInterpretation = CreateControlInterpretation(
            "$.properties.address.properties.streetNumber",
            "$.properties.streetNumber",
            "$.properties.address",
            rule: null
        );
        var streetNumberContext = contextFactory.Create(streetNumberInterpretation, "$.address");

        var sut = new FormRuleEnforcer();

        // Pre-Assert
        Assert.That(streetContext.Disabled, Is.EqualTo(initialDisabledValue));

        // Act
        sut.EnforceRule(dataContext, streetContext, [streetContext, streetNumberContext]);

        // Assert
        Assert.That(streetContext.Disabled, Is.EqualTo(expectedDisabledValue));
    }

    [Test]
    public void When_EnforceRulesForPages_And_RuleMustBeEnforced_Then_Applies_Effect_To_PageUnderEvaluation()
    {
        // Arrange
        var formData = new JObject
        {
            ["address"] = new JObject
            {
                ["streetNumber"] = "12"
            }
        };
        var dataContext = JsonFormDataContextBuilder.BuildAndInstantiate(
            JSchema.Parse(jsonSchema),
            formData
        );

        var streetNumberInterpretation = CreateControlInterpretation(
            "$.properties.address.properties.streetNumber",
            "$.properties.streetNumber",
            "$.properties.address",
            rule: null
        );
        var streetNumberContext = contextFactory.Create(streetNumberInterpretation, "$.address");

        var rule = new UiSchemaRuleInterpretation(
            "$.properties.address.properties.streetNumber",
            JSchema.Parse("{\"const\": \"12\"}"),
            UiSchemaElementRuleEffect.Hide
        );
        var pageInterpretation = new UiSchemaPageInterpretation(
            false,
            false,
            false,
            streetNumberInterpretation,
            null,
            rule
        );
        var pageContext = contextFactory
            .CreatePages([pageInterpretation])
            .First();

        var sut = new FormRuleEnforcer();

        // Pre-Assert
        Assert.That(pageContext.Hidden, Is.False);

        // Act
        sut.EnforceRulesForPages(dataContext, [pageContext], [streetNumberContext]);

        // Assert
        Assert.That(pageContext.Hidden, Is.True);
    }

    private static UiSchemaControlInterpretation CreateControlInterpretation(string absoluteSchemaPath, string relativeSchemaPath, string? parentSchemaPath, UiSchemaRuleInterpretation? rule, bool initialHiddenValue = false, bool initialDisabledValue = false)
    {
        return new(
            ControlType.String,
            new(null, null),
            false,
            initialDisabledValue,
            initialHiddenValue,
            relativeSchemaPath,
            absoluteSchemaPath,
            absoluteSchemaPath.Split('.').Last(),
            parentSchemaPath,
            new(UiSchema.UiSchemaElementType.Control, null, null, [], "#/properties/address/properties/street", null, null),
            rule
        );
    }
}