using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Routing;

namespace Ramses.Utils
{
    /// <summary>
    /// Contains extension methods that provides custom Url helpers
    /// </summary>
    /// <History>
    /// <Created Date="2013/07/30" Author="BPlus">CR - 127361 - Projeto PF MAP (http://jira.scania.com/browse/NET-116)</Created>
    /// </History>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates a fully qualified URL to the current request setting new language value route.
        /// </summary>
        /// <param name="url">The UrlHelper instance that this method extends.</param>
        /// <param name="language">The name of the language to be set</param>
        /// <param name="request">the System.Web.HttpRequest object for the current HTTP request</param>
        /// <param name="context">An instance of ViewContext with information that is used to render the view</param>
        /// <returns></returns>
        public static string ActionLanguage(this UrlHelper url, string language, HttpRequestBase request, ViewContext context)
        {
            StringBuilder result = new StringBuilder();
            RouteValueDictionary routeValues = new RouteValueDictionary();

            context.RouteData.Values.ToList().ForEach(v => routeValues.Add(v.Key, v.Key == "culture" ? language : v.Value));

            result.Append(url.Action(context.Controller.ValueProvider.GetValue("action").RawValue.ToString(),
                    context.Controller.ValueProvider.GetValue("controller").RawValue.ToString(), routeValues));

            if (request.QueryString.HasKeys())
            {
                result.Append("?");
                result.Append(string.Join("&", request.QueryString.AllKeys.Select(k => string.Format("{0}={1}", k, request.QueryString[k])).ToList()));
            }

            return result.ToString();
        }
    }
}