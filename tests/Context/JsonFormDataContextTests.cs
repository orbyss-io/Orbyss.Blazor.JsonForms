using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Orbyss.Blazor.JsonForms.Context.Models;
using Orbyss.Blazor.JsonForms.Context.Utils;
using Orbyss.Blazor.JsonForms.Interpretation;

namespace Orbyss.Blazor.JsonForms.Tests.Context
{
    [TestFixture]
    public sealed class JsonFormDataContextTests
    {
        private const string schemaJson = "{\"properties\":{\"firstName\":{\"type\":\"string\", \"minLength\": 50}}, \"required\":[\"firstName\"]}";
        private static readonly JSchema schema = JSchema.Parse(schemaJson);

        [Test]
        public void When_GetValue_Then_Returns_Value_For_ControlContext()
        {
            // Arrange
            var formData = new JObject
            {
                ["firstName"] = "Johannes"
            };
            var sut = JsonFormDataContextBuilder.BuildAndInstantiate(
                schema,
                formData
            );
            var formControl = GetFormContext();

            // Act
            var result = sut.GetValue(formControl);

            // Assert
            Assert.That($"{result}", Is.EqualTo("Johannes"));
        }

        [Test]
        public void When_SetValue_Then_Returns_Value_For_ControlContext()
        {
            // Arrange
            var formData = new JObject
            {
                ["firstName"] = "Johannes"
            };
            var sut = JsonFormDataContextBuilder.BuildAndInstantiate(
                schema,
                formData
            );
            var formControl = GetFormContext();

            // Act
            sut.UpdateValue(formControl, JValue.CreateString("Hendrik"));

            // Assert
            var result = sut.GetValue(formControl);
            Assert.That($"{result}", Is.EqualTo("Hendrik"));
        }

        [Test]
        public void When_Validate_Then_SetsErrorMessagesWhenInvalid()
        {
            // Arrange
            var formData = new JObject
            {
                ["firstName"] = "Johannes"
            };
            var sut = JsonFormDataContextBuilder.BuildAndInstantiate(
                schema,
                formData
            );
            var formControl = GetFormContext();

            // Act
            var isValid = sut.Validate([formControl]);

            // Assert
            Assert.That(isValid, Is.False);

            var errors = formControl.Errors.ToArray();
            Assert.That(errors, Has.Length.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo(ErrorType.MinimumLength));
        }

        [Test]
        public void When_Validate_And_Is_Required_SetsErrorMessagesWhenInvalid()
        {
            // Arrange
            var formData = new JObject();
            var sut = JsonFormDataContextBuilder.BuildAndInstantiate(
                schema,
                formData
            );
            var formControl = GetFormContext();

            // Act
            var isValid = sut.Validate([formControl]);

            // Assert
            Assert.That(isValid, Is.False);

            var errors = formControl.Errors.ToArray();
            Assert.That(errors, Has.Length.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo(ErrorType.Required));
        }

        private static FormControlContext GetFormContext()
        {
            var formControlInterpretation = new UiSchemaControlInterpretation(
                ControlType.String,
                new UiSchemaLabelInterpretation(null, null),
                false,
                false,
                false,
                "$.properties.firstName",
                "$.properties.firstName",
                "firstName",
                "$",
                new UiSchema.FormUiSchemaElement(UiSchema.UiSchemaElementType.Control, null, null, [], "#/properties/firstName", null, null),
                null
            );
            return new FormControlContext("$.firstName", "$", formControlInterpretation);
        }
    }
}