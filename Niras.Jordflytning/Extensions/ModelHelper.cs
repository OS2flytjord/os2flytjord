using System;
using System.Linq.Expressions;
using System.Web.Mvc;

public static partial class ModelHelper
{

    public static string GetPropertyName<TModel, TValue>(this TModel model, Expression<Func<TModel, TValue>> propertySelector)
    {
        return ExpressionHelper.GetExpressionText(propertySelector);
    }

}
