using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DFC.Digital.Web.Core.HtmlExtentions
{
    public static class BreadCrumbNavigationExtension
    {
        public static MvcHtmlString BuildBreadcrumbNavigation(this HtmlHelper helper)
        {
            string area = (helper.ViewContext.RouteData.DataTokens["area"] ?? string.Empty).ToString();
            string controller = helper.ViewContext.RouteData.Values["controller"].ToString();
            string action = helper.ViewContext.RouteData.Values["action"].ToString();
            string urlName = helper.ViewContext.RouteData.Values["urlName"].ToString();

            // add link to homepage by default
            StringBuilder breadcrumb = new StringBuilder(@"
                <ol class='breadcrumb'>
                    <li>" + helper.ActionLink(urlName, action, controller) + @"</li>");

            // add link to area if existing
            if (area != string.Empty)
            {
                breadcrumb.Append("<li>");
                if (ControllerExistsInArea("Default", area))
                {
                    breadcrumb.Append(helper.ActionLink(area.AddSpaceOnCaseChange(), action, controller, new { Area = area }, new { @class = string.Empty }));
                }
                else
                {
                    breadcrumb.Append(area.AddSpaceOnCaseChange());
                }

                breadcrumb.Append("</li>");
            }

            // add link to controller Index if different action
            if ((controller != "Home" && controller != "Default") && action != "Index")
            {
                if (ActionExistsInController("Index", controller, area))
                {
                    breadcrumb.Append("<li>");
                    breadcrumb.Append(helper.ActionLink(controller.AddSpaceOnCaseChange(), action, controller, new { Area = area }, new { @class = string.Empty }));
                    breadcrumb.Append("</li>");
                }
            }

            // add link to action
            if ((controller != "Home" && controller != "Default") || action != "Index")
            {
                breadcrumb.Append("<li>");

                //breadcrumb.Append(helper.ActionLink((action.ToLower() == "index") ? controller.AddSpaceOnCaseChange() : action.AddSpaceOnCaseChange(), action, controller, new { Area = area }, new { @class = "" }));
                breadcrumb.Append((action.ToLower() == "index") ? controller.AddSpaceOnCaseChange() : action.AddSpaceOnCaseChange());
                breadcrumb.Append("</li>");
            }

            return MvcHtmlString.Create(breadcrumb.Append("</ol>").ToString());
        }

        public static Type GetControllerType(string controller, string area)
        {
            var currentAssembly = Assembly.GetExecutingAssembly().GetName().Name;
            var controllerTypes = Assembly.GetExecutingAssembly().GetTypes().Where(o => typeof(IController).IsAssignableFrom(o));

            string typeFullName = $"{currentAssembly}.Controllers.{controller}Controller";
            if (area != string.Empty)
            {
                typeFullName = $"{currentAssembly}.Areas.{area}.Controllers.{controller}Controller";
            }

            return controllerTypes.FirstOrDefault(o => o.FullName == typeFullName);
        }

        public static bool ActionExistsInController(string action, string controller, string area)
        {
            Type controllerType = GetControllerType(controller, area);
            return controllerType != null && new ReflectedControllerDescriptor(controllerType).GetCanonicalActions().Any(x => x.ActionName == action);
        }

        public static bool ControllerExistsInArea(string controller, string area)
        {
            Type controllerType = GetControllerType(controller, area);
            return controllerType != null;
        }

    public static string AddSpaceOnCaseChange(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                {
                    newText.Append(' ');
                }

                newText.Append(text[i]);
        }

        return newText.ToString();
    }
}

    //public static class BreadCrumbNavigationExtension
    //{
    //    public static string BuildBreadcrumbNavigation(this HtmlHelper helper, string action, string controller)
    //    {
    //        // optional condition: I didn't wanted it to show on home and account controller
    //        if (helper.ViewContext.RouteData.Values["controller"].ToString() == "Home" ||
    //            helper.ViewContext.RouteData.Values["controller"].ToString() == "Account")
    //        {
    //            return string.Empty;
    //        }

    //        StringBuilder breadcrumb = new StringBuilder("<ol class='breadcrumb'><li>").Append(helper.ActionLink("Explore a career", action.ToString(), controller.ToString()).ToHtmlString()).Append("</li>");
    //        breadcrumb.Append("<li>");
    //        breadcrumb.Append(helper.ActionLink(
    //            helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
    //            "Index",
    //            helper.ViewContext.RouteData.Values["controller"].ToString().Titleize()));
    //        breadcrumb.Append("</li>");

    //        if (helper.ViewContext.RouteData.Values["action"].ToString() != "Index")
    //        {
    //            breadcrumb.Append("<li>");
    //            breadcrumb.Append(helper.ActionLink(
    //                helper.ViewContext.RouteData.Values["action"].ToString(),
    //                helper.ViewContext.RouteData.Values["action"].ToString(),
    //                helper.ViewContext.RouteData.Values["controller"].ToString()));
    //            breadcrumb.Append("</li>");
    //        }

    //        return breadcrumb.Append("</ol>").ToString();
    //    }
    //}
}
