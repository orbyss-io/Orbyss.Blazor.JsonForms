using Orbyss.Components.Json.Models;
using Orbyss.Blazor.JsonForms.Interpretation;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Orbyss.Blazor.JsonForms.Constants
{
    public static class ControlTypeLookup
    {
        public static readonly Type Enum = typeof(string);

        public static readonly Type EnumList = typeof(IEnumerable<string>);

        public static readonly Type DateTime = typeof(DateTime?);

        public static readonly Type DateTimeUtcTicks = typeof(DateTimeUtcTicks?);

        public static readonly Type DateOnly = typeof(DateOnly?);

        public static readonly Type DateOnlyUtcTicks = typeof(DateUtcTicks?);

        public static readonly Type Number = typeof(double?);

        public static readonly Type Integer = typeof(int?);

        public static readonly Type String = typeof(string);

        public static readonly Type Boolean = typeof(bool);

        private static readonly ReadOnlyDictionary<ControlType, Type> fieldsPerControlType = typeof(ControlTypeLookup)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .ToDictionary(x => System.Enum.Parse<ControlType>(x.Name, true), x => (Type)x.GetValue(null)!)
            .AsReadOnly();

        public static Type GetForControlType(ControlType controlType)
        {
            if (!fieldsPerControlType.TryGetValue(controlType, out var result))
            {
                throw new NotSupportedException($"Cannot lookup System.Type for control type '{controlType}'");
            }

            return result;
        }
    }
}