using System.Reflection;

namespace Orbyss.Components.JsonForms.Utils
{
    public static class RemoveUndeclaredParameters
    {
        public static void Remove(Type componentType, IDictionary<string, object?> parameters)
        {
            var parameterDeclarations = componentType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<Microsoft.AspNetCore.Components.ParameterAttribute>() is not null)
                .Select(x => x.Name);

            var parametersThatAreNotDeclared = parameters.Keys.Where(x => !parameterDeclarations.Contains(x));
            foreach (var undeclaredParameter in parametersThatAreNotDeclared)
            {
                parameters.Remove(undeclaredParameter);
            }
        }

        public static void Remove<TComponentType>(IDictionary<string, object?> parameters)
        {
            Remove(typeof(TComponentType), parameters);
        }
    }
}
