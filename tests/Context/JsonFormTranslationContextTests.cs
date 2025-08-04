using Newtonsoft.Json.Schema;
using Orbyss.Components.Json.Models;
using Orbyss.Blazor.JsonForms.Context.Translations;
using Orbyss.Blazor.JsonForms.Context.Utils;
using Orbyss.Blazor.JsonForms.Interpretation;
using System.Reflection;
using System.Text.Json.Nodes;

namespace Orbyss.Blazor.JsonForms.Tests.Context
{
    [TestFixture]
    public sealed class JsonFormTranslationContextTests
    {
        private const string jsonSchema = "{\"properties\":{\"address\":{\"type\":\"object\",\"properties\":{\"street\":{\"type\":\"string\"}}},\"addressType\":{\"type\":\"string\",\"enum\":[\"H\",\"B\",\"S\"]},\"nonTranslatedEnum\":{\"type\":\"string\",\"enum\":[\"First\",\"Second\",\"Third\"]}}}";
        private static readonly JSchema schema = JSchema.Parse(jsonSchema);

        private static readonly TranslationSchema translationSchema = new(
            new TranslationResourcesDictionary
            {
                ["en"] = new TranslationSchemaResource(
                    new JsonObject
                    {
                        ["address"] = new JsonObject
                        {
                            ["label"] = "Address Test Label",
                            ["street"] = new JsonObject
                            {
                                ["label"] = "Street Test Label"
                            }
                        },
                        ["customI18nSection"] = new JsonObject
                        {
                            ["label"] = "Some custom i18n translation"
                        },
                        ["addressType"] = new JsonObject
                        {
                            ["label"] = "Address type",
                            ["H"] = "House",
                            ["B"] = "Business",
                            ["S"] = "School",
                            ["error"] = new JsonObject
                            {
                                ["minimumLength"] = "Must have min length of 10",
                                ["required"] = "Must make a choice"
                            }
                        },
                        ["nonTranslatedEnum"] = new JsonObject
                        {
                            ["label"] = "Non Translated"
                        }
                    }
                )
            }
        );

        [Test]
        public void When_Instantiate_Then_SetsTranslationObjects()
        {
            // Act
            var sut = JsonFormTranslationContextBuilder.BuildAndInstantiate(
                translationSchema,
                schema
            );

            // Assert
            var translations = sut
                .GetType()
                .GetField("translations", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(sut)
                as TranslationObject[];

            Assert.That(translations, Is.Not.Null);
            Assert.That(translations, Has.Length.EqualTo(1));
            Assert.That(translations.Any(x => x.Language == "en"), Is.True);
            Assert.That(translations[0].Sections, Has.Count.GreaterThanOrEqualTo(1));
            Assert.That(translations[0].Sections.ContainsKey("address"), Is.True);
        }

        [Test]
        public void When_TranslateLabel_ForNestedControl_Then_ReturnsTransactionSectionLabel()
        {
            // Arrange
            var controlInterpretation = new UiSchemaControlInterpretation(
                ControlType.String,
                new(null, null),
                false,
                false,
                false,
                "$.properties.street",
                "$.properties.address.properties.street",
                "street",
                "$.properties.address",
                new(UiSchema.UiSchemaElementType.Control, null, null, [], "#/properties/address/properties/street", null, null),
                null
            );
            var sut = JsonFormTranslationContextBuilder.BuildAndInstantiate(
                translationSchema,
                schema
            );

            // Act
            var result = sut.TranslateLabel("en", controlInterpretation);

            // Assert
            Assert.That(result, Is.EqualTo("Street Test Label"));
        }

        [Test]
        public void When_TranslateLabel_ForControl_And_LanguageUnknown_Then_Returns_HumanReadablePropertyName()
        {
            // Arrange
            var controlInterpretation = new UiSchemaControlInterpretation(
                ControlType.String,
                new(null, null),
                false,
                false,
                false,
                "$.properties.street",
                "$.properties.address.properties.street",
                "street",
                "$.properties.address",
                new(UiSchema.UiSchemaElementType.Control, null, null, [], "#/properties/address/properties/street", null, null),
                null
            );
            var sut = JsonFormTranslationContextBuilder.BuildAndInstantiate(
                translationSchema,
                schema
            );

            // Act
            var result = sut.TranslateLabel("nl", controlInterpretation);

            // Assert
            Assert.That(result, Is.EqualTo("Street"));
        }

        [Test]
        public void When_TranslateLabel_ForControl_And_LanguageNull_Then_Returns_HumanReadablePropertyName()
        {
            // Arrange
            var controlInterpretation = new UiSchemaControlInterpretation(
                ControlType.String,
                new(null, null),
                false,
                false,
                false,
                "$.properties.street",
                "$.properties.address.properties.street",
                "street",
                "$.properties.address",
                new(UiSchema.UiSchemaElementType.Control, null, null, [], "#/properties/address/properties/street", null, null),
                null
            );
            var sut = JsonFormTranslationContextBuilder.BuildAndInstantiate(
                translationSchema,
                schema
            );

            // Act
            var result = sut.TranslateLabel(null, controlInterpretation);

            // Assert
            Assert.That(result, Is.EqualTo("Street"));
        }

        [Test]
        public void When_TranslateLabel_ForControl_And_SectionNotFound_Then_Returns_HumanReadablePropertyName()
        {
            // Arrange
            var controlInterpretation = new UiSchemaControlInterpretation(
                ControlType.String,
                new(null, null),
                false,
                false,
                false,
                "$.properties.unknownNestedProp",
                "$.properties.unknownProp.properties.unknownNestedProp",
                "unknownNestedProp",
                "$.properties.unknownProp",
                new(UiSchema.UiSchemaElementType.Control, null, null, [], "#/properties/unknownProp/properties/unknownNestedProp", null, null),
                null
            );
            var sut = JsonFormTranslationContextBuilder.BuildAndInstantiate(
                translationSchema,
                schema
            );

            // Act
            var result = sut.TranslateLabel(null, controlInterpretation);

            // Assert
            Assert.That(result, Is.EqualTo("Unknown nested prop"));
        }

        [Test]
        public void When_TranslateEnum_ForControl_Then_ReturnsTranslatedEnums()
        {
            // Arrange
            var controlInterpretation = new UiSchemaControlInterpretation(
                ControlType.String,
                new(null, null),
                false,
                false,
                false,
                "$.properties.addressType",
                "$.properties.addressType",
                "addressType",
                "$",
                new(UiSchema.UiSchemaElementType.Control, null, null, [], "#/properties/addressType", null, null),
                null
            );
            var sut = JsonFormTranslationContextBuilder.BuildAndInstantiate(
                translationSchema,
                schema
            );

            // Act
            var result = sut
                .TranslateEnum("en", controlInterpretation)?
                .ToArray();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Length.EqualTo(3));
            Assert.That(result[0].Value, Is.EqualTo("H"));
            Assert.That(result[0].Label, Is.EqualTo("House"));
            Assert.That(result[1].Value, Is.EqualTo("B"));
            Assert.That(result[1].Label, Is.EqualTo("Business"));
            Assert.That(result[2].Value, Is.EqualTo("S"));
            Assert.That(result[2].Label, Is.EqualTo("School"));
        }

        [Test]
        public void When_TranslateEnum_ForControl_And_NoTranslationsConfigured_Then_ReturnsDefaultEnumTranslations()
        {
            // Arrange
            var controlInterpretation = new UiSchemaControlInterpretation(
                ControlType.String,
                new(null, null),
                false,
                false,
                false,
                "$.properties.nonTranslatedEnum",
                "$.properties.nonTranslatedEnum",
                "nonTranslatedEnum",
                "$",
                new(UiSchema.UiSchemaElementType.Control, null, null, [], "#/properties/nonTranslatedEnum", null, null),
                null
            );
            var sut = JsonFormTranslationContextBuilder.BuildAndInstantiate(
                translationSchema,
                schema
            );

            // Act
            var result = sut
                .TranslateEnum("en", controlInterpretation)?
                .ToArray();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Length.EqualTo(3));
            Assert.That(result[0].Value, Is.EqualTo("First"));
            Assert.That(result[0].Label, Is.EqualTo("First"));
            Assert.That(result[1].Value, Is.EqualTo("Second"));
            Assert.That(result[1].Label, Is.EqualTo("Second"));
            Assert.That(result[2].Value, Is.EqualTo("Third"));
            Assert.That(result[2].Label, Is.EqualTo("Third"));
        }

        [Test]
        public void When_TranslateErrors_ForControl_Then_ReturnsErrorTranslation()
        {
            // Arrange
            var controlInterpretation = new UiSchemaControlInterpretation(
                ControlType.String,
                new(null, null),
                false,
                false,
                false,
                "$.properties.addressType",
                "$.properties.addressType",
                "addressType",
                "$",
                new(UiSchema.UiSchemaElementType.Control, null, null, [], "#/properties/addressType", null, null),
                null
            );
            var sut = JsonFormTranslationContextBuilder.BuildAndInstantiate(
                translationSchema,
                schema
            );
            var errorTypes = new ErrorType[]
            {
                ErrorType.Required,
                ErrorType.MinimumLength
            };

            // Act
            var result = sut.TranslateErrors("en", errorTypes, controlInterpretation);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo("Must make a choice. Must have min length of 10"));
        }

        [Test]
        public void When_TranslateErrors_ForControl_And_NoErrorTranslationsConfigured_Then_ReturnsDefaultErrorTranslation()
        {
            // Arrange
            var controlInterpretation = new UiSchemaControlInterpretation(
                ControlType.String,
                new(null, null),
                false,
                false,
                false,
                "$.properties.nonTranslatedEnum",
                "$.properties.nonTranslatedEnum",
                "nonTranslatedEnum",
                "$",
                new(UiSchema.UiSchemaElementType.Control, null, null, [], "#/properties/nonTranslatedEnum", null, null),
                null
            );
            var sut = JsonFormTranslationContextBuilder.BuildAndInstantiate(
                translationSchema,
                schema
            );
            var errorTypes = new ErrorType[]
            {
                ErrorType.Required,
                ErrorType.MinimumLength
            };

            // Act
            var result = sut.TranslateErrors("en", errorTypes, controlInterpretation);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo($"{DefaultJsonFormValidationMessages.Required}. {DefaultJsonFormValidationMessages.MinLength}"));
        }

        [Test]
        public void When_TranslateLabel_ForLabelInterpretation_Then_ReturnsTransactionSectionLabel()
        {
            // Arrange
            var sut = JsonFormTranslationContextBuilder.BuildAndInstantiate(
                translationSchema,
                schema
            );
            var labelInterpretation = new UiSchemaLabelInterpretation(
                "address",
                null
            );

            // Act
            var result = sut.TranslateLabel("en", labelInterpretation);

            // Assert
            Assert.That(result, Is.EqualTo("Address Test Label"));
        }

        [Test]
        public void When_TranslateLabel_ForLabelInterpretation_Includingi18N_Then_ReturnsTransactionSectionLabel()
        {
            // Arrange
            var sut = JsonFormTranslationContextBuilder.BuildAndInstantiate(
                translationSchema,
                schema
            );
            var labelInterpretation = new UiSchemaLabelInterpretation(
                "label_is_ignored",
                "customI18nSection"
            );

            // Act
            var result = sut.TranslateLabel("en", labelInterpretation);

            // Assert
            Assert.That(result, Is.EqualTo("Some custom i18n translation"));
        }
    }
}