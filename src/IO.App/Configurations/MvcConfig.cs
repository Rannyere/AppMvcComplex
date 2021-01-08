using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace IO.App.Configurations
{
    public static class MvcConfig
    {
        public static IServiceCollection AddMvcConfiguration(this IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                o.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "The completed value is invalid for this field.");
                o.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "This field needs to be filled. ");
                o.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Este campo precisa ser preenchido.");
                o.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "It is necessary that the body in the request is not empty.");
                o.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => "The completed value is invalid for this field.");
                o.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "The completed value is invalid for this field.");
                o.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "O campo deve ser numérico");
                o.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => "The field must be numeric");
                o.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => "The completed value is invalid for this field.");
                o.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "The field must be numeric.");
                o.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "This field needs to be filled.");

                o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
                

            return services;
        }
    }
}
