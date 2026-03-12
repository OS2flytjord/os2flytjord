using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

// ReSharper disable once CheckNamespace
public static partial class LabelHelper
{

    /// <summary>
    /// Wraps html in a label tag
    /// </summary>
    /// <param name="html"></param>
    /// <param name="text"></param>
    /// <param name="tooltip"></param>
    /// <param name="htmlAttributes"></param>
    /// <returns></returns>
    public static IHtmlString WithLabel(this IHtmlString html, string text, string tooltip, RouteValueDictionary htmlAttributes)
    {
        var label = new TagBuilder("label");
        var attributes = (htmlAttributes ?? new RouteValueDictionary());
        
        if (attributes.Any())
            label.MergeAttributes(attributes);
        
        if (!string.IsNullOrWhiteSpace(tooltip))
            label.MergeAttribute("title", tooltip);

        return MvcHtmlString.Create(string.Format("{0}{1}{2}{3}", label.ToString(TagRenderMode.StartTag), html, text, label.ToString(TagRenderMode.EndTag)));
    }

    /// <summary>
    /// Wraps html in a label tag
    /// </summary>
    /// <param name="html"></param>
    /// <param name="text"></param>
    /// <param name="htmlAttributes"></param>
    /// <returns></returns>
    public static IHtmlString WithLabel(this IHtmlString html, string text, RouteValueDictionary htmlAttributes)
    {
        return WithLabel(html, text, null, htmlAttributes);
    }

    /// <summary>
    /// Wraps html in a label tag
    /// </summary>
    /// <param name="html"></param>
    /// <param name="text"></param>
    /// <param name="tooltip"></param>
    /// <param name="htmlAttributes"></param>
    /// <returns></returns>
    public static IHtmlString WithLabel(this IHtmlString html, string text, string tooltip, dynamic htmlAttributes)
    {
        RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        return WithLabel(html, text, tooltip, attributes);
    }

    /// <summary>
    /// Wraps html in a label tag
    /// </summary>
    /// <param name="html"></param>
    /// <param name="text"></param>
    /// <param name="htmlAttributes"></param>
    /// <returns></returns>
    public static IHtmlString WithLabel(this IHtmlString html, string text, dynamic htmlAttributes)
    {
        return WithLabel(html, text, null, htmlAttributes);
    }


}
