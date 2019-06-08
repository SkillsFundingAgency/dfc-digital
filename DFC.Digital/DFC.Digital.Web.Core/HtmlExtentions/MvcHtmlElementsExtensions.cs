using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DFC.Digital.Web.Core
{
    public static class MvcHtmlElementsExtensions
    {
        public static MvcHtmlString GovUkEnumRadioButtonFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
            where TModel : class
        {
            var enumType = typeof(TProperty);
            if (enumType.IsNullableEnum())
            {
                enumType = Nullable.GetUnderlyingType(enumType);
            }

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
                stringBuilder.AppendLine("<div class=\"govuk-radios__item\">");
                stringBuilder.AppendLine(htmlHelper.RadioButtonFor(expression, item.Value, new { @class = "govuk-radios__input", id = elementId }).ToHtmlString());
                stringBuilder.AppendLine(htmlHelper.LabelFor(expression, item.DisplayName, new { @class = "govuk-label govuk-radios__label", @for = elementId }).ToHtmlString());
                stringBuilder.AppendLine("</div>");
            }

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString GovUkEnumConditionalRadioButtonFor<TModel, TProperty>(
          this HtmlHelper<TModel> htmlHelper,
          Expression<Func<TModel, TProperty>> expression,
          string ariaConditionalName)
          where TModel : class
        {
            var enumType = typeof(TProperty);
            if (enumType.IsNullableEnum())
            {
                enumType = Nullable.GetUnderlyingType(enumType);
            }

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
                stringBuilder.AppendLine("<div class=\"govuk-radios__item\">");
                stringBuilder.AppendLine(htmlHelper.RadioButtonFor(expression, item.Value, new { @class = "govuk-radios__input", id = elementId, aria_controls = ariaConditionalName, aria_expanded= "false" }).ToHtmlString());
                stringBuilder.AppendLine(htmlHelper.LabelFor(expression, item.DisplayName, new { @class = "govuk-label govuk-radios__label", @for = elementId }).ToHtmlString());
                stringBuilder.AppendLine("</div>");
            }

            return new MvcHtmlString(stringBuilder.ToString());
        }

        /// <summary>
        /// Labels the with hint for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString LabelWithHintFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var tagText = metadata.DisplayName;

            if (string.IsNullOrEmpty(tagText))
            {
                return new MvcHtmlString(string.Empty);
            }

            TagBuilder tag = new TagBuilder("label");

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            // If forId is supplied we override 'for' attribute value
            if (attributes.ContainsKey("forId"))
            {
                object idValue;
                attributes.TryGetValue("forId", out idValue);
                string idStrValue = idValue as string;
                fieldId = idStrValue;
            }

            tag.Attributes.Add("for", fieldId);

            if (attributes.ContainsKey("class"))
            {
                object value;
                attributes.TryGetValue("class", out value);
                string classValue = value as string;
                tag.Attributes.Add("class", classValue);
            }

            tag.SetInnerText(tagText);

            return new MvcHtmlString(HttpUtility.HtmlDecode(tag.ToString(TagRenderMode.Normal)));
        }

        /// <summary>
        /// Labels the with hint for using hidden field.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString LabelWithHintForUsingHiddenField<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var tagText = metadata.DisplayName;
            if (string.IsNullOrEmpty(tagText))
            {
                return new MvcHtmlString(string.Empty);
            }

            TagBuilder tag = new TagBuilder("label");

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            if (attributes.ContainsKey("class"))
            {
                object value;
                attributes.TryGetValue("class", out value);
                string classValue = value as string;
                tag.Attributes.Add("class", classValue);
            }

            tag.SetInnerText(tagText);

            return new MvcHtmlString(HttpUtility.HtmlDecode(tag.ToString(TagRenderMode.Normal)));
        }

        public static string GetErrorClass(this HtmlHelper htmlHelper, string propertyName, ModelStateDictionary modelState)
        {
            if (modelState.IsValid == false)
            {
                return modelState.ContainsKey(propertyName) && modelState[propertyName].Errors.Count > 0 ? "govuk-form-group--error" : string.Empty;
            }

            return string.Empty;
        }

        public static bool IsNullableEnum(this Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }

        public static MvcHtmlString CheckBoxForSimple<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object htmlAttributes)
        {
            var checkBoxWithHidden = htmlHelper.CheckBoxFor(expression, htmlAttributes).ToHtmlString();
            var pureCheckBox = checkBoxWithHidden.Substring(0, checkBoxWithHidden.IndexOf("<input", 1, StringComparison.InvariantCultureIgnoreCase));
            return new MvcHtmlString(pureCheckBox);
        }
    }
}