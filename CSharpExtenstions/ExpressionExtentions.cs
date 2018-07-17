using System.Linq.Expressions;
using System.Reflection;

namespace CSharpExtenstions
{
    public class ExpressionHelper : ExpressionVisitor
    {
        private MemberExpression _memberExpression;

        public MemberExpression GetPropertyAccessExpression(Expression expression)
        {
            _memberExpression = null;

            Visit(expression);

            return _memberExpression;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var property = node.Member as PropertyInfo;

            if (property != null)
                _memberExpression = node;

            return base.VisitMember(node);
        }
    }
}