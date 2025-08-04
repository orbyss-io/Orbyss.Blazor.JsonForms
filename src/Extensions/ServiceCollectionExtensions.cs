using Microsoft.Extensions.DependencyInjection;
using Orbyss.Components.JsonForms.Context;
using Orbyss.Components.JsonForms.Context.Interfaces;
using Orbyss.Components.JsonForms.Context.Notifications;
using Orbyss.Components.JsonForms.Interpretation;
using Orbyss.Components.JsonForms.Interpretation.Interfaces;

namespace Orbyss.Components.JsonForms.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonForms(
            this IServiceCollection services, 
            Func<IServiceProvider, IJsonFormContext>? jsonFormContextFactory = null            
        )
        {            
            services
                .AddScoped<IControlTypeInterpreter, ControlTypeInterpreter>()
                .AddScoped<IJsonPathInterpreter, JsonPathInterpreter>()
                .AddScoped<IFormUiSchemaInterpreter, FormUiSchemaInterpreter>()
                .AddScoped<IJsonTransformer, JlioJsonTransformer>()
                .AddScoped<IFormRuleEnforcer, FormRuleEnforcer>()
                .AddScoped<IFormElementContextFactory, FormElementContextFactory>()
                    
                .AddTransient<IJsonFormNotificationHandler, JsonFormNotificationHandler>()
                .AddTransient<IJsonFormDataContext, JsonFormDataContext>()
                .AddTransient<IJsonFormTranslationContext, JsonFormTranslationContext>();

            if (jsonFormContextFactory is not null)
            {
                services.AddTransient(jsonFormContextFactory);
            }
            else
            {
                services.AddTransient<IJsonFormContext, JsonFormContext>();
            }

            return services;
        }
    }
}
