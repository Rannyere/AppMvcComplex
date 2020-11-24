using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace IO.App.Extensions
{
    public class CurrencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var moeda = Convert.ToDecimal(value, new CultureInfo("de-DE"));
            }
            catch (Exception)
            {
                return new ValidationResult("Currency in invalid format");
            }

            return ValidationResult.Success;
        }
    }

    public class CurrencyAttributeAdapter : AttributeAdapterBase<CurrencyAttribute>
    {

        public CurrencyAttributeAdapter(CurrencyAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {

        }
        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-currency", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(context));
        }
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return "Currency in invalid format";
        }
    }
    public class CurrencyValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();
        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is CurrencyAttribute currencyAttribute)
            {
                return new CurrencyAttributeAdapter(currencyAttribute, stringLocalizer);
            }

            return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
