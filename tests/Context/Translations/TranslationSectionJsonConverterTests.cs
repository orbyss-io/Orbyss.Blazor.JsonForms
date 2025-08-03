using Orbyss.Components.Json.Models;
using Orbyss.Components.JsonForms.Context.Translations;
using System.Text.Json;

namespace Orbyss.Components.JsonForms.Tests.Context.Translations
{
    [TestFixture]
    public class TranslationSectionJsonConverterTests
    {
        const string translationSchemaJson = "{\"resources\":{\"en\":{\"translation\":{\"firstName\":{\"label\":\"First name\",\"minimumLength\":\"First name must require at least 1 character\"},\"surname\":{\"label\":\"Last name\",\"error\":{\"required\":\"Last name is required\"}},\"birthPlace\":{\"label\":\"place of birth\",\"home\":\"At home\",\"hospital\":\"In a hospital bed\",\"unknown\":\"Nobody is sure\"},\"address\":{\"label\":\"Address\",\"street\":{\"label\":\"Street name\"},\"streetNumber\":{\"label\":\"Street number\"}},\"someCustomLabel\":\"Here we can directly add a translation label\"}},\"nl\":{\"translation\":{\"firstName\":{\"label\":\"Voornaam\",\"minLength\":\"Voornaam moet op zijn minst 1 karakter bevatten\"},\"surname\":{\"label\":\"Achternaam\",\"error\":{\"required\":\"Achternaam is een verplicht veld\"}},\"birthPlace\":{\"label\":\"Geboorteplaats\",\"home\":\"Thuis\",\"hospital\":\"In een ziekenhuis\",\"unknown\":\"Niemand weet het zeker\"},\"address\":{\"label\":\"Adres\",\"street\":{\"label\":\"Straatnaam\"},\"streetNumber\":{\"label\":\"Straatnummer\"}},\"someCustomLabel\":\"Hier kunnen we direct een label vertaling plaatsen\"}}}}";
        const string translationSections = "{\"firstName\":{\"label\":\"First name\",\"error\":{\"minimumLength\":\"First name must require at least 1 character\"}},\"surname\":{\"label\":\"Last name\",\"error\":{\"required\":\"Last name is required\"}},\"birthPlace\":{\"label\":\"place of birth\",\"home\":\"At home\",\"hospital\":\"In a hospital bed\",\"unknown\":\"Nobody is sure\"},\"address\":{\"label\":\"Address\",\"street\":{\"label\":\"Street name\"},\"streetNumber\":{\"label\":\"Street number\"}},\"someCustomLabel\":\"Here we can directly add a translation label\"}";
        static readonly JsonSerializerOptions options = GetSerializerOptions();

        [Test]
        public void When_Deserialize_Then_Returns_TranslationSection()
        {            
            // Act
            var result = JsonSerializer.Deserialize<Dictionary<string, TranslationSection>>(translationSections, options);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(5));
            Assert.That(result.ContainsKey("firstName"), Is.True);
            Assert.That(result.ContainsKey("address"), Is.True);
            Assert.That(result.ContainsKey("someCustomLabel"), Is.True);

            Assert.That(result["firstName"].Label, Is.EqualTo("First name"));
            Assert.That(result["firstName"].Error, Is.Not.Null);
            Assert.That(result["firstName"].Error.MinimumLength, Is.EqualTo("First name must require at least 1 character"));

            Assert.That(result["address"].NestedSections, Is.Not.Null);
            Assert.That(result["address"].NestedSections, Has.Count.EqualTo(2));
            Assert.That(result["address"].NestedSections["street"].Label, Is.EqualTo("Street name"));
            Assert.That(result["address"].NestedSections["streetNumber"].Label, Is.EqualTo("Street number"));

            Assert.That(result["someCustomLabel"].Label, Is.EqualTo("Here we can directly add a translation label"));
        }

        [Test]
        public void When_Serialize_Then_Returns_Json()
        {
            // Arrange
            const string sectionJson = "{\"label\":\"TEST\",\"error\":{},\"Some Value\":\"Some Label\",\"nestedSection\":{\"label\":\"NESTED_TEST\",\"error\":{},\"Some Nested Value\":\"Some Nested Label\"}}";
            var section = new TranslationSection(
                "TEST",
                TranslationErrorSection.DefaultSection(),
                [ 
                    new ("Some Label", "Some Value") 
                ],
                new Dictionary<string, TranslationSection>
                {
                    ["nestedSection"]  = new TranslationSection(
                        "NESTED_TEST",
                        TranslationErrorSection.DefaultSection(),
                        [
                            new ("Some Nested Label", "Some Nested Value")
                        ],
                        null
                    )
                }
            );

            // Act
            var result = JsonSerializer.Serialize(section, options);

            // Assert
            Assert.That(
                result, 
                Is.EqualTo(sectionJson)
            );
        }

        static JsonSerializerOptions GetSerializerOptions()
        {
            var result = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            result.Converters.Add(new TranslationSectionJsonConverter());
            return result;
        }
    }
}
