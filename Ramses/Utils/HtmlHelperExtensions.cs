using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ramses.Utils
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Returns HTML markup for image with action link
        /// </summary>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="action">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller .</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <param name="imagePath">A virtual path to the image file.</param>
        /// <param name="htmlAttributes">An object that contains additional attributes for the html markup</param>
        /// <returns></returns>
        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, string controllerName, object routeValues, string imagePath, object htmlAttributes)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            // build the <a> tag
            var anchorBuilder = new TagBuilder("a");
            anchorBuilder.MergeAttribute("href", url.Action(action, controllerName, routeValues));
            anchorBuilder.InnerHtml = BuildImageTag(html, imagePath, htmlAttributes).ToHtmlString(); // include the <img> tag inside

            return MvcHtmlString.Create(anchorBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Returns HTML markup for image 
        /// </summary>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="imagePath">A virtual path to the image file.</param>
        /// <param name="htmlAttributes">An object that contains additional attributes for the html markup</param>
        /// <returns></returns>
        public static MvcHtmlString Image(this HtmlHelper html, string imagePath, object htmlAttributes)
        {
            return BuildImageTag(html, imagePath, htmlAttributes);
        }

        /// <summary>
        /// Method that creates a tag for image
        /// </summary>
        /// <param name="imgUrl">Url for image</param>
        /// <param name="htmlAttributes">An object that contains additional attributes for the html markup</param>
        /// <returns>Tag with image</returns>
        private static MvcHtmlString BuildImageTag(HtmlHelper html, string imagePath, object htmlAttributes)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            TagBuilder imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url.Content(imagePath));
            imgBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return MvcHtmlString.Create(imgBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}