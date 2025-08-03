using Newtonsoft.Json.Linq;
using Orbyss.Components.JsonForms.Context.Models;
using Orbyss.Components.JsonForms.Context.Notifications;
using Orbyss.Components.JsonForms.Context.Utils;

namespace Orbyss.Components.JsonForms.Tests.Context
{
    [TestFixture]
    public sealed class JsonFormContextTests
    {
        private const string jsonSchema = "{\"properties\":{\"firstName\":{\"type\":\"string\"},\"surname\":{\"type\":\"string\"}}}";
        private const string translationSchema = "{\"resources\":{\"en\":{\"translation\":{\"firstName\":{\"label\":\"First Name\"}}},\"nl\":{\"translation\":{\"firstName\":{\"label\":\"Voornaam\"}}}}}";
        private const string uiSchema = "{\"type\":\"VerticalLayout\",\"elements\":[{\"type\":\"Control\",\"scope\":\"#/properties/firstName\",\"options\":{\"readonly\":true,\"disabled\":true}},{\"type\":\"Control\",\"scope\":\"#/properties/surname\",\"options\":{\"hidden\":true},\"rule\":{\"effect\":\"Show\",\"condition\":{\"scope\":\"#/properties/firstName\",\"schema\":{\"minLength\":2}}}}],\"options\":{\"customOption\":\"custom-option-value\"}}";

        [Test]
        public void When_Instantiate_Then_SetsUpContext()
        {
            // Arrange
            var formData = new JObject
            {
                ["firstName"] = "H"
            };

            var initOptions = new JsonFormContextInitOptions(
                jsonSchema,
                uiSchema,
                translationSchema
            )
            {
                Language = "nl",
                Disabled = true,
                ReadOnly = true,
                Data = formData
            };

            // Act
            var sut = JsonFormContextBuilder.BuildAndInstantiate(initOptions);

            // Assert
            Assert.That(sut.Disabled, Is.True);
            Assert.That(sut.ReadOnly, Is.True);

            var page = sut.GetPage(0);
            var verticalLayout = (FormVerticalLayoutContext)page.ElementContexts[0];
            var surnameElement = verticalLayout.Rows.First(x => x.Interpretation.Label?.Label == "surname");
            Assert.That(surnameElement.Hidden, Is.True);

            var formOption = sut.GetFormOption("customOption");
            Assert.That($"{formOption}", Is.EqualTo("custom-option-value"));
        }

        [Test]
        public void When_ChangeDisabled_Then_PublishesEvent()
        {
            // Arrange            
            int assertionValue = 0;
            var initOptions = new JsonFormContextInitOptions(
                jsonSchema,
                uiSchema,
                translationSchema
            );
            var sut = JsonFormContextBuilder.BuildAndInstantiate(initOptions);
            var subscriptionToken = sut.FormNotification.Subscribe(
                JsonFormNotificationType.OnDisabledChanged,
                () => assertionValue = 12
            );

            // Act
            sut.ChangeDisabled(true);

            // Assert
            Assert.That(assertionValue, Is.EqualTo(12));
        }

        [Test]
        public void When_ChangeLanguage_Then_PublishesEvent()
        {
            // Arrange            
            int assertionValue = 0;
            var initOptions = new JsonFormContextInitOptions(
                jsonSchema,
                uiSchema,
                translationSchema
            );
            var sut = JsonFormContextBuilder.BuildAndInstantiate(initOptions);
            var subscriptionToken = sut.FormNotification.Subscribe(
                JsonFormNotificationType.OnLanguageChanged,
                () => assertionValue = 12
            );

            // Act
            sut.ChangeLanguage("en");

            // Assert
            Assert.That(assertionValue, Is.EqualTo(12));
        }

        [Test]
        public void When_ChangeReadOnly_Then_PublishesEvent()
        {
            // Arrange            
            int assertionValue = 0;
            var initOptions = new JsonFormContextInitOptions(
                jsonSchema,
                uiSchema,
                translationSchema
            );
            var sut = JsonFormContextBuilder.BuildAndInstantiate(initOptions);
            var subscriptionToken = sut.FormNotification.Subscribe(
                JsonFormNotificationType.OnReadOnlyChanged,
                () => assertionValue = 12
            );

            // Act
            sut.ChangeReadOnly(true);

            // Assert
            Assert.That(assertionValue, Is.EqualTo(12));
        }

        [Test]
        public void When_Validate_Then_PublishesEvent()
        {
            // Arrange            
            int assertionValue = 0;
            var initOptions = new JsonFormContextInitOptions(
                jsonSchema,
                uiSchema,
                translationSchema
            );
            var sut = JsonFormContextBuilder.BuildAndInstantiate(initOptions);
            var subscriptionToken = sut.FormNotification.Subscribe(
                JsonFormNotificationType.OnDataValidated,
                () => assertionValue = 12
            );

            // Act
            _ = sut.Validate();

            // Assert
            Assert.That(assertionValue, Is.EqualTo(12));
        }

        [Test]
        public void When_GetValue_Then_ReturnsControlJsonToken()
        {
            // Arrange
            var formData = new JObject
            {
                ["firstName"] = "Johannes"
            };

            var initOptions = new JsonFormContextInitOptions(jsonSchema, uiSchema, translationSchema)
            {
                Data = formData
            };
            var sut = JsonFormContextBuilder.BuildAndInstantiate(initOptions);
            var page = sut.GetPage(0);
            var verticalLayout = (FormVerticalLayoutContext)page.ElementContexts[0];
            var firstNameContext = verticalLayout.Rows.First(x => x.Interpretation.Label?.Label == "firstName");

            // Act            
            var result = sut.GetValue(firstNameContext.Id);

            // Assert
            Assert.That($"{result}", Is.EqualTo("Johannes"));
        }

        [Test]
        public void When_GetValue_And_ContextIsNotFound_Then_ThrowsException()
        {
            // Arrange            
            var invalidId = Guid.NewGuid();
            var initOptions = new JsonFormContextInitOptions(jsonSchema, uiSchema, translationSchema);
            var sut = JsonFormContextBuilder.BuildAndInstantiate(initOptions);

            // Act & Assert
            var e = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = sut.GetValue(invalidId);
            });
            Assert.That(e.Message, Is.EqualTo($"Could not find context by id '{invalidId}'"));
        }

        [Test]
        public void When_GetValue_And_ContextIsNotControl_Then_ThrowsException()
        {
            // Arrange            
            var initOptions = new JsonFormContextInitOptions(jsonSchema, uiSchema, translationSchema);
            var sut = JsonFormContextBuilder.BuildAndInstantiate(initOptions);

            var page = sut.GetPage(0);
            var verticalLayout = (FormVerticalLayoutContext)page.ElementContexts[0];

            // Act & Assert
            var e = Assert.Throws<InvalidCastException>(() =>
            {
                _ = sut.GetValue(verticalLayout.Id);
            });
            Assert.That(e.Message, Is.EqualTo($"Context of type '{verticalLayout.GetType()}' could not be cast to type '{typeof(FormControlContext)}'"));
        }



        [Test]
        public void When_UpdateValue_Then_UpdatesContextToken_And_EnforcesRules()
        {
            // Arrange
            var formData = new JObject
            {
                ["firstName"] = "H"
            };

            var initOptions = new JsonFormContextInitOptions(jsonSchema, uiSchema, translationSchema)
            {
                Data = formData
            };
            var sut = JsonFormContextBuilder.BuildAndInstantiate(initOptions);
            var page = sut.GetPage(0);
            var verticalLayout = (FormVerticalLayoutContext)page.ElementContexts[0];
            var firstNameContext = verticalLayout.Rows.First(x => x.Interpretation.Label?.Label == "firstName");
            var surnameElement = verticalLayout.Rows.First(x => x.Interpretation.Label?.Label == "surname");

            // Pre-Assert
            Assert.That(surnameElement.Hidden, Is.True);

            // Act            
            sut.UpdateValue(firstNameContext.Id, JValue.CreateString("Johannes"));

            // Assert
            var updatedFirstNameToken = sut.GetValue(firstNameContext.Id);

            Assert.That($"{updatedFirstNameToken}", Is.EqualTo("Johannes"));
            Assert.That(surnameElement.Hidden, Is.False);
        }
    }
}
