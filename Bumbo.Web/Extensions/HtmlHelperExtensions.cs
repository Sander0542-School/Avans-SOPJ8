using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bumbo.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controllers, string actions = null, string cssClass = "active")
        {
            var currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;
            var currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;

            IEnumerable<string> acceptedControllers = (controllers ?? currentController ?? "").Split(',');
            IEnumerable<string> acceptedActions = (actions ?? currentAction ?? "").Split(',');

            return acceptedControllers.Contains(currentController) && (acceptedActions.Contains(currentAction) || currentAction == null) ? cssClass : string.Empty;
        }

        public static string IsActiveArea(this IHtmlHelper htmlHelper, string area, string pages = null, string cssClass = "active")
        {
            var currentArea = htmlHelper.ViewContext.RouteData.Values["area"] as string;
            var currentPage = htmlHelper.ViewContext.RouteData.Values["page"] as string;

            IEnumerable<string> acceptedAreas = (area ?? currentArea ?? "").Split(',');
            IEnumerable<string> acceptedPages = (pages ?? currentPage ?? "").Split(',');

            return acceptedAreas.Contains(currentArea) && (acceptedPages.Contains(currentPage) || currentPage == null) ? cssClass : string.Empty;
        }

        public static HtmlString ConditionalAttr(this IHtmlHelper helper, string name, string value)
        {
            return helper.ConditionalAttr(name, value, true);
        }

        public static HtmlString ConditionalAttr(this IHtmlHelper helper, string name, string value, bool render)
        {
            return helper.ConditionalAttr(name, value, () => render);
        }

        public static HtmlString ConditionalAttr(this IHtmlHelper helper, string name, string value, Func<bool> condition)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                return new HtmlString(string.Empty);
            }

            return condition?.Invoke() ?? false ? new HtmlString($"{name}=\"{HttpUtility.HtmlAttributeEncode(value)}\"") : new HtmlString(string.Empty);
        }
    }
}