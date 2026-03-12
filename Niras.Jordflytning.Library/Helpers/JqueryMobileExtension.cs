using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
	public static class JqueryMobileExtension
	{
		public static MvcHtmlString FlipSwitchFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			bool value = (bool)(metadata.Model ?? false);

			List<SelectListItem> items =
					new List<SelectListItem>()
                    {

                        new SelectListItem() { Text = "Nej", Value = "False", Selected = (!value) },
                        new SelectListItem() { Text = "Ja", Value = "True", Selected = (value) }
                    };

			return htmlHelper.DropDownListFor(expression, items, new { data_role = "slider"}); //,  data_mini="true"
		}
	}
}
