using Newtonsoft.Json.Schema;
using Orbyss.Components.JsonForms.Interpretation;
using Orbyss.Components.JsonForms.UiSchema;
using System.Text.Json.Nodes;

namespace Orbyss.Components.JsonForms.Tests.Interpretation
{
    [TestFixture]
    public sealed class FormUiSchemaInterpreterTests
    {
        [Test]
        public void When_Interpret_Then_Returns_UiSchemaInterpretation()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"firstName\":{\"type\":\"string\"}}}";
            var schema = JSchema.Parse(schemaJson);
            var uiSchema = new FormUiSchema(
              UiSchemaElementType.VerticalLayout,
              null,
              null,
              [
                  new FormUiSchemaElement(UiSchemaElementType.Control, null, null, [], "#/properties/firstName", null, null)
              ],
              null
            );
            var sut = GetSut();

            // Act
            var result = sut.Interpret(uiSchema, schema);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Pages, Has.Length.EqualTo(1));
            Assert.That(result.Pages[0].InterpretedElements, Has.Length.EqualTo(1));

            var firstVerticalLayoutElement = result.Pages[0].InterpretedElements[0] as UiSchemaVerticalLayoutInterpretation;
            Assert.That(firstVerticalLayoutElement, Is.Not.Null);

            var firstRowControlElement = firstVerticalLayoutElement.Rows[0] as UiSchemaControlInterpretation;
            Assert.That(firstRowControlElement, Is.Not.Null);
            Assert.That(firstRowControlElement.Label?.Label, Is.EqualTo("firstName"));
        }

        [Test]
        public void When_Interpret_And_CategorizationChildElements_NotContainOnlyCategories_Then_ThrowsException()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"firstName\":{\"type\":\"string\"}}}";
            var schema = JSchema.Parse(schemaJson);
            var uiSchema = new FormUiSchema(
              UiSchemaElementType.Categorization,
              null,
              null,
              [
                  new FormUiSchemaElement(UiSchemaElementType.Control, null, null, [], "#/properties/firstName", null, null)
              ],
              null
            );
            var sut = GetSut();

            // Act & Assert
            var e = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = sut.Interpret(uiSchema, schema);
            });
            Assert.That(e.Message, Is.EqualTo("For a UI Schema of type categorization, all direct child elements must be of type Category"));
        }

        [Test]
        public void When_Interpret_And_HorizontalLayout_DoesNotHave_ChildElements_Then_ThrowsException()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"firstName\":{\"type\":\"string\"}}}";
            var schema = JSchema.Parse(schemaJson);
            var uiSchema = new FormUiSchema(
              UiSchemaElementType.HorizontalLayout,
              null,
              null,
              [
              ],
              null
            );
            var sut = GetSut();

            // Act & Assert
            var e = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = sut.Interpret(uiSchema, schema);
            });
            Assert.That(e.Message, Is.EqualTo("Horizontal layout element must have elements defined"));
        }

        [Test]
        public void When_Interpret_And_VerticalLayout_DoesNotHave_ChildElements_Then_ThrowsException()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"firstName\":{\"type\":\"string\"}}}";
            var schema = JSchema.Parse(schemaJson);
            var uiSchema = new FormUiSchema(
              UiSchemaElementType.VerticalLayout,
              null,
              null,
              [
              ],
              null
            );
            var sut = GetSut();

            // Act & Assert
            var e = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = sut.Interpret(uiSchema, schema);
            });
            Assert.That(e.Message, Is.EqualTo("Vertical layout element must have elements defined"));
        }

        [Test]
        public void When_Interpret_Then_Sets_Disabled_ReadOnly_And_Hidden_From_Options()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"firstName\":{\"type\":\"string\"}}}";
            var schema = JSchema.Parse(schemaJson);
            var uiSchema = new FormUiSchema(
                UiSchemaElementType.VerticalLayout,
                null,
                null,
                [
                    new FormUiSchemaElement(
                        UiSchemaElementType.Control,
                        null,
                        null,
                        [],
                        "#/properties/firstName",
                        null,
                        new JsonObject
                        {
                            ["hidden"] = true,
                            ["disabled"] = true,
                            ["readonly"] = true
                        }
                    )
                ],
                new JsonObject
                {
                    ["hidden"] = true,
                    ["disabled"] = true,
                    ["readonly"] = true
                }
            );
            var sut = GetSut();

            // Act
            var result = sut.Interpret(uiSchema, schema);

            // Assert
            Assert.That(result, Is.Not.Null);

            var page = result.Pages[0];
            Assert.That(page.Disabled, Is.True);
            Assert.That(page.Hidden, Is.True);
            Assert.That(page.ReadOnly, Is.True);

            var firstVerticalLayoutElement = result.Pages[0].InterpretedElements[0] as UiSchemaVerticalLayoutInterpretation;
            Assert.That(firstVerticalLayoutElement, Is.Not.Null);

            var firstRowControlElement = firstVerticalLayoutElement.Rows[0] as UiSchemaControlInterpretation;
            Assert.That(firstRowControlElement, Is.Not.Null);
            Assert.That(firstRowControlElement.Disabled, Is.True);
            Assert.That(firstRowControlElement.Hidden, Is.True);
            Assert.That(firstRowControlElement.ReadOnly, Is.True);
        }

        private static FormUiSchemaInterpreter GetSut()
        {
            return new FormUiSchemaInterpreter(
                new JsonPathInterpreter(),
                new ControlTypeInterpreter()
            );
        }
    }
}