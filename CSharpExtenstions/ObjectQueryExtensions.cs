using System;
using System.Linq.Expressions;
namespace CSharpExtenstions
{
    public static class ObjectQueryExtensions
    {
       
        private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpr = expression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException("Expression body must be a member expression");
            return memberExpr.Member.Name;
        }
    }
}
