using Newtonsoft.Json.Schema;
using Orbyss.Blazor.JsonForms.Extensions;
using Orbyss.Components.Json.Models;

namespace Orbyss.Blazor.JsonForms.Tests.Context.Translations;

[TestFixture]
public class TranslationErrorSectionTests
{
    [Test]
    public void When_GetValue_Then_Returns_Value_For_ErrorType()
    {
        // Arrange
        var sut = new TranslationErrorSection(
            Custom: null,
            Const: "Const error message",
            Required: "Required error message",
            Minimum: "Minimum error",
            Maximum: "Maximum error",
            MinimumLength: "Min length error",
            MaximumLength: "Max length error",
            MinimumItems: "Min items error",
            MaximumItems: "Max items error",
            Contains: "Contains error",
            Pattern: "Pattern error"
        );

        // Act
        var constError = sut.GetTranslatedError(ErrorType.Const);
        var requiredError = sut.GetTranslatedError(ErrorType.Required);
        var minimumError = sut.GetTranslatedError(ErrorType.Minimum);
        var maximumError = sut.GetTranslatedError(ErrorType.Maximum);
        var minLengthError = sut.GetTranslatedError(ErrorType.MinimumLength);
        var maxLengthError = sut.GetTranslatedError(ErrorType.MaximumLength);
        var minItemsError = sut.GetTranslatedError(ErrorType.MinimumItems);
        var maxItemsError = sut.GetTranslatedError(ErrorType.MaximumItems);
        var containsError = sut.GetTranslatedError(ErrorType.Contains);
        var patternError = sut.GetTranslatedError(ErrorType.Pattern);

        // Assert
        Assert.That(constError, Is.EqualTo(sut.Const));
        Assert.That(requiredError, Is.EqualTo(sut.Required));
        Assert.That(minimumError, Is.EqualTo(sut.Minimum));
        Assert.That(maximumError, Is.EqualTo(sut.Maximum));
        Assert.That(minLengthError, Is.EqualTo(sut.MinimumLength));
        Assert.That(maxLengthError, Is.EqualTo(sut.MaximumLength));
        Assert.That(minItemsError, Is.EqualTo(sut.MinimumItems));
        Assert.That(maxItemsError, Is.EqualTo(sut.MaximumItems));
        Assert.That(containsError, Is.EqualTo(sut.Contains));
        Assert.That(patternError, Is.EqualTo(sut.Pattern));
    }

    [Test]
    public void When_GetValue_And_CustomMessageSpecified_Then_Returns_CustomMessage()
    {
        // Arrange
        var sut = new TranslationErrorSection(
            Custom: "Custom message",
            Const: null,
            Required: null,
            Minimum: null,
            Maximum: null,
            MinimumLength: null,
            MaximumLength: null,
            MinimumItems: null,
            MaximumItems: null,
            Contains: null,
            Pattern: null
        );

        // Act
        var constError = sut.GetTranslatedError(ErrorType.Const);
        var requiredError = sut.GetTranslatedError(ErrorType.Required);
        var minimumError = sut.GetTranslatedError(ErrorType.Minimum);
        var maximumError = sut.GetTranslatedError(ErrorType.Maximum);
        var minLengthError = sut.GetTranslatedError(ErrorType.MinimumLength);
        var maxLengthError = sut.GetTranslatedError(ErrorType.MaximumLength);
        var minItemsError = sut.GetTranslatedError(ErrorType.MinimumItems);
        var maxItemsError = sut.GetTranslatedError(ErrorType.MaximumItems);
        var containsError = sut.GetTranslatedError(ErrorType.Contains);
        var patternError = sut.GetTranslatedError(ErrorType.Pattern);

        // Assert
        Assert.That(constError, Is.EqualTo(sut.Custom));
        Assert.That(requiredError, Is.EqualTo(sut.Custom));
        Assert.That(minimumError, Is.EqualTo(sut.Custom));
        Assert.That(maximumError, Is.EqualTo(sut.Custom));
        Assert.That(minLengthError, Is.EqualTo(sut.Custom));
        Assert.That(maxLengthError, Is.EqualTo(sut.Custom));
        Assert.That(minItemsError, Is.EqualTo(sut.Custom));
        Assert.That(maxItemsError, Is.EqualTo(sut.Custom));
        Assert.That(containsError, Is.EqualTo(sut.Custom));
        Assert.That(patternError, Is.EqualTo(sut.Custom));
    }

    [Test]
    public void When_GetValue_And_CustomMessageSpecified_ButErrorSpecificMessageAlsoSpecified_Then_Returns_ErrorTypeSpecificMessage()
    {
        // Arrange
        var sut = new TranslationErrorSection(
            Custom: "Custom message",
            Const: "Const message",
            Required: null,
            Minimum: null,
            Maximum: null,
            MinimumLength: null,
            MaximumLength: null,
            MinimumItems: null,
            MaximumItems: null,
            Contains: null,
            Pattern: null
        );

        // Act
        var constError = sut.GetTranslatedError(ErrorType.Const);

        // Assert
        Assert.That(constError, Is.EqualTo(sut.Const));
    }

    [Test]
    public void When_GetValue_And_ErrorSectionIsDefault_ThenReturns_DefaultMessages()
    {
        // Arrange
        var sut = TranslationErrorSection.DefaultSection();

        // Act
        var constError = sut.GetTranslatedError(ErrorType.Const);
        var requiredError = sut.GetTranslatedError(ErrorType.Required);
        var minimumError = sut.GetTranslatedError(ErrorType.Minimum);
        var maximumError = sut.GetTranslatedError(ErrorType.Maximum);
        var minLengthError = sut.GetTranslatedError(ErrorType.MinimumLength);
        var maxLengthError = sut.GetTranslatedError(ErrorType.MaximumLength);
        var minItemsError = sut.GetTranslatedError(ErrorType.MinimumItems);
        var maxItemsError = sut.GetTranslatedError(ErrorType.MaximumItems);
        var containsError = sut.GetTranslatedError(ErrorType.Contains);
        var patternError = sut.GetTranslatedError(ErrorType.Pattern);

        // Assert
        Assert.That(constError, Is.EqualTo(DefaultJsonFormValidationMessages.Const));
        Assert.That(requiredError, Is.EqualTo(DefaultJsonFormValidationMessages.Required));
        Assert.That(minimumError, Is.EqualTo(DefaultJsonFormValidationMessages.Minimum));
        Assert.That(maximumError, Is.EqualTo(DefaultJsonFormValidationMessages.Maximum));
        Assert.That(minLengthError, Is.EqualTo(DefaultJsonFormValidationMessages.MinLength));
        Assert.That(maxLengthError, Is.EqualTo(DefaultJsonFormValidationMessages.MaxLength));
        Assert.That(minItemsError, Is.EqualTo(DefaultJsonFormValidationMessages.MinItems));
        Assert.That(maxItemsError, Is.EqualTo(DefaultJsonFormValidationMessages.MaxItems));
        Assert.That(containsError, Is.EqualTo(DefaultJsonFormValidationMessages.Contains));
        Assert.That(patternError, Is.EqualTo(DefaultJsonFormValidationMessages.Pattern));
    }
}