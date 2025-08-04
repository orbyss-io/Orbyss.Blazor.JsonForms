using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace Orbyss.Blazor.JsonForms.ComponentInstances
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

        internal string? Language { get; set; }

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

        protected virtual IDictionary<string, object?> GetButtonParameters()
        {
            return new Dictionary<string, object?>();
        }

        protected override sealed IDictionary<string, object?> GetParametersCore()
        {
            var result = GetButtonParameters();

            result[nameof(Text)] = Text;
            result[nameof(Disabled)] = Disabled;
            result[nameof(OnClicked)] = OnClicked;

            return result;
        }
    }
}