using Newtonsoft.Json.Schema;
using Orbyss.Components.JsonForms.Interpretation;
using Orbyss.Components.JsonForms.Interpretation.Exceptions;

namespace Orbyss.Components.JsonForms.Tests.Interpretation
{
    [TestFixture]
    public sealed class ControlTypeInterpreterTests
    {
        [Test]
        public void When_Interpret_And_ThereIsNoSchemaSectionForControl_Then_ThrowsException()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"firstName\":{\"type\":\"string\"}}}";
            const string unknownAbsolutePath = "$.properties.unknownProperty";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act & Assert
            Assert.Throws<InvalidSchemaTypeConfigurationException>(() =>
            {
                _ = sut.Interpret(schema, unknownAbsolutePath, null);
            });
        }

        [Test]
        public void When_Interpret_And_SchemaTypeDoesNotHaveValue_Then_ThrowsException()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"firstName\":{ }}}";
            const string absolutePath = "$.properties.firstName";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act & Assert
            Assert.Throws<SchemaTypeNotSpecifiedException>(() =>
            {
                _ = sut.Interpret(schema, absolutePath, null);
            });
        }

        [Test]
        public void When_HandleStringType_And_FormatEqualsDateTime_Then_ReturnsDateTimeControlType()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"birthDate\":{\"type\": \"string\",\"format\": \"datetime\" }}}";
            const string absolutePath = "$.properties.birthDate";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act
            var result = sut.Interpret(schema, absolutePath, null);

            Assert.That(result, Is.EqualTo(ControlType.DateTime));
        }

        [Test]
        public void When_HandleStringType_And_FormatEqualsDate_Then_ReturnsDateOnlyControlType()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"birthDate\":{\"type\": \"string\",\"format\": \"date\" }}}";
            const string absolutePath = "$.properties.birthDate";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act
            var result = sut.Interpret(schema, absolutePath, null);

            Assert.That(result, Is.EqualTo(ControlType.DateOnly));
        }

        [Test]
        public void When_HandleStringType_And_IsEnum_Then_ReturnsEnumControlType()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"birthDateType\":{\"type\": \"string\",\"enum\": [\"early\", \"late\"] }}}";
            const string absolutePath = "$.properties.birthDateType";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act
            var result = sut.Interpret(schema, absolutePath, null);

            Assert.That(result, Is.EqualTo(ControlType.Enum));
        }

        [Test]
        public void When_HandleStringType_And_IsEnumArray_Then_ReturnsEnumControlType()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"birthDateType\":{\"type\":\"array\",\"items\":{\"type\":\"string\",\"enum\":[\"early\",\"late\"]}}}}";
            const string absolutePath = "$.properties.birthDateType.items";
            const string absoluteParentPath = "$.properties.birthDateType";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act
            var result = sut.Interpret(schema, absolutePath, absoluteParentPath);

            Assert.That(result, Is.EqualTo(ControlType.EnumList));
        }

        [Test]
        public void When_HandleStringType_Then_ReturnsStringControlType()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"birthDate\":{\"type\": \"string\" }}}";
            const string absolutePath = "$.properties.birthDate";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act
            var result = sut.Interpret(schema, absolutePath, null);

            Assert.That(result, Is.EqualTo(ControlType.String));
        }

        [Test]
        public void When_HandleNumericType_And_FormatEqualsDateTime_Then_ReturnsDateTimeControlType()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"birthDate\":{\"type\": \"number\",\"format\": \"datetime\" }}}";
            const string absolutePath = "$.properties.birthDate";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act
            var result = sut.Interpret(schema, absolutePath, null);

            Assert.That(result, Is.EqualTo(ControlType.DateTimeUtcTicks));
        }

        [Test]
        public void When_HandleNumericType_And_FormatEqualsDate_Then_ReturnsDateOnlyControlType()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"birthDate\":{\"type\": \"number\",\"format\": \"date\" }}}";
            const string absolutePath = "$.properties.birthDate";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act
            var result = sut.Interpret(schema, absolutePath, null);

            Assert.That(result, Is.EqualTo(ControlType.DateOnlyUtcTicks));
        }

        [Test]
        public void When_HandleNumericType_Then_ReturnsNumericControlType()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"birthDate\":{\"type\": \"number\"}}}";
            const string absolutePath = "$.properties.birthDate";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act
            var result = sut.Interpret(schema, absolutePath, null);

            Assert.That(result, Is.EqualTo(ControlType.Number));
        }

        [Test]
        public void When_JschemaTypeBoolean_Then_ReturnsBooleanControlType()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"booleanValue\":{\"type\": \"boolean\"}}}";
            const string absolutePath = "$.properties.booleanValue";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act
            var result = sut.Interpret(schema, absolutePath, null);

            Assert.That(result, Is.EqualTo(ControlType.Boolean));
        }

        [Test]
        public void When_JschemaTypeInteger_Then_ReturnsIntegerControlType()
        {
            // Arrange
            const string schemaJson = "{\"properties\":{\"intValue\":{\"type\": \"integer\"}}}";
            const string absolutePath = "$.properties.intValue";
            var schema = JSchema.Parse(schemaJson);
            var sut = new ControlTypeInterpreter();

            // Act
            var result = sut.Interpret(schema, absolutePath, null);

            Assert.That(result, Is.EqualTo(ControlType.Integer));
        }
    }
}