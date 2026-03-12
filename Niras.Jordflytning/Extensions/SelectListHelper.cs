using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

// ReSharper disable once CheckNamespace
public static partial class SelectListHelper
{

    /// <summary>
    /// Fixes the stupid mising text and value bindings for a SelectList when created from SelectListItems
    /// </summary>
    /// <param name="items">The items</param>
    /// <param name="selectedValue">Optional selected value</param>
    /// <returns></returns>
    public static SelectList ToSelectList(this IEnumerable<SelectListItem> items, object selectedValue = null)
    {
        return new SelectList(items, "Value", "Text", selectedValue);
    }
}