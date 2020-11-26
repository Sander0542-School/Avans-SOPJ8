using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bumbo.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controllers, string actions = null, string cssClass = "active")
        {
            string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;
            string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;

            IEnumerable<string> acceptedControllers = (controllers ?? currentController ?? "").Split(',');
            IEnumerable<string> acceptedActions = (actions ?? currentAction ?? "").Split(',');

            return acceptedControllers.Contains(currentController) && (acceptedActions.Contains(currentAction) || currentAction == null) ? cssClass : String.Empty;
        }

        public static string IsActiveArea(this IHtmlHelper htmlHelper, string area, string pages = null, string cssClass = "active")
        {
            string currentArea = htmlHelper.ViewContext.RouteData.Values["area"] as string;
            string currentPage = htmlHelper.ViewContext.RouteData.Values["page"] as string;

            IEnumerable<string> acceptedAreas = (area ?? currentArea ?? "").Split(',');
            IEnumerable<string> acceptedPages = (pages ?? currentPage ?? "").Split(',');

            return acceptedAreas.Contains(currentArea) && (acceptedPages.Contains(currentPage) || currentPage == null) ? cssClass : String.Empty;
        }
    }
}