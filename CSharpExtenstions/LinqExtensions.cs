using CSharpExtenstions.Helpers;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CSharpExtenstions
{
    public static class LinqExtensions
    {

        public static PropertyInfo ExtractPropertyInfo(this LambdaExpression propertyAccessor)
        {
            return propertyAccessor.ExtractMemberInfo() as PropertyInfo;
        }

        public static FieldInfo ExtractFieldInfo(this LambdaExpression propertyAccessor)
        {
            return propertyAccessor.ExtractMemberInfo() as FieldInfo;
        }

        public static MemberInfo ExtractMemberInfo(this LambdaExpression propertyAccessor)
        {
            Guard.ArgumentNotNull(() => propertyAccessor);

            MemberInfo info;
            try
            {
                MemberExpression operand;
                LambdaExpression expression = propertyAccessor;

                if (expression.Body is UnaryExpression)
                {

                    UnaryExpression body = (UnaryExpression)expression.Body;

                    operand = (MemberExpression)body.Operand;
                }
                else
                {

                    operand = (MemberExpression)expression.Body;
                }

                MemberInfo member = operand.Member;
                info = member;
            }
            catch (Exception e)
            {
                throw new ArgumentException("The property or field accessor expression is not in the expected format 'o => o.PropertyOrField'.", e);
            }

            return info;
        }

    }
}