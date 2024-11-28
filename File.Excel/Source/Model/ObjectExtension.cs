using System;
using System.Linq.Expressions;

namespace App.File.Excel
{
    internal static class ObjectExtension
    {
        public static string PropertyName<T, TProperty>(this Expression<Func<T, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression != null)
            {
                return memberExpression.Member.Name;
            }

            var unaryExpression = expression.Body as UnaryExpression;

            if (unaryExpression != null)
            {
                var member = unaryExpression.Operand as MemberExpression;
                if (member != null)
                {
                    return member.Member.Name;
                }
            }

            throw new InvalidOperationException("Expression does not represent a valid property.");
        }
    }
}