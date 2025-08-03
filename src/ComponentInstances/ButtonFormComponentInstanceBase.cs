using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace Orbyss.Components.JsonForms.ComponentInstances
{
    public abstract class ButtonFormComponentInstanceBase : FormComponentInstanceBase
    {
        private readonly ReadOnlyDictionary<string, string>? translations;
        private readonly string? text;

        protected ButtonFormComponentInstanceBase(IDictionary<string, string> textTranslations)
        {
            var dictionary = new Dictionary<string, string>(textTranslations, StringComparer.OrdinalIgnoreCase);
            translations = new ReadOnlyDictionary<string, string>(dictionary);
        }

        protected ButtonFormComponentInstanceBase(string text)
        {
            this.text = text;
        }

        public string? Language { get; internal set; }

        public EventCallback? OnClicked { get; internal set; }

        public bool Disabled { get; set; }

        public string? Text
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    return text;
                }

                if (string.IsNullOrWhiteSpace(Language))
                {
                    return null;
                }

                if (translations?.ContainsKey(Language) == true)
                {
                    return translations[Language];
                }

                return null;
            }
        }

        protected override IDictionary<string, object?> GetParametersCore()
        {
            return new Dictionary<string, object?>
            {
                [nameof(Text)] = Text,
                [nameof(Disabled)] = Disabled,
                [nameof(OnClicked)] = OnClicked
            };
        }
    }
}