using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DFC.Digital.Web.Core.HtmlExtentions
{
    public static class MvcHtmlElementsExtensions
    {
        public static MvcHtmlString GovUkEnumRadioButtonFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
            where TModel : class
        {
            var enumType = typeof(TProperty);
            var enumEntryNames = Enum.GetNames(enumType);
            var entries = enumEntryNames
                .Select(n => new
                {
                    Name = n,
                    DisplayAttribute = enumType
                        .GetField(n)
                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                        .OfType<DisplayAttribute>()
                        .SingleOrDefault() ?? new DisplayAttribute()
                })
                .Select(e => new
                {
                    Value = e.Name,
                    DisplayName = e.DisplayAttribute.Name ?? e.Name,
                    Order = e.DisplayAttribute.GetOrder() ?? 0
                })
                .OrderBy(e => e.Order)
                .ThenBy(e => e.Value);

            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in entries)
            {
                var elementId = $"{metaData.PropertyName}_{item.Value}";
                stringBuilder.AppendLine("<div class=\"govuk - radios__item\">");
                stringBuilder.AppendLine(htmlHelper.RadioButtonFor(expression, item.Value, new { @class = "govuk-radios__input", id = elementId }).ToHtmlString());
                stringBuilder.AppendLine(htmlHelper.LabelFor(expression, item.DisplayName, new { @class = "govuk-label govuk-radios__label", @for = elementId }).ToHtmlString());
                stringBuilder.AppendLine("</div>");
            }

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static string GetErrorClass(this System.Web.Mvc.HtmlHelper htmlHelper, string propertyName, ModelStateDictionary modelState)
        {
            if (modelState.IsValid == false)
            {
                return modelState.ContainsKey(propertyName) && modelState[propertyName].Errors.Count > 0 ? "govuk-form-group--error" : string.Empty;
            }

            return string.Empty;
        }
    }
}