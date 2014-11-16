using System;
using System.Linq.Expressions;

namespace Mailgun.Exceptions
{
    internal static class ThrowIf
    {
        public static void IsPropertyNull<T>(Expression<Func<T>> expression) where T : class
        {
            if (expression.Compile().Invoke() == null)
            {
                throw new RequiredPropertyNullException(GetName(expression));
            }
        }
        
        public static void IsArgumentNull<T>(Expression<Func<T>> expression) where T : class
        {
            if (expression.Compile().Invoke() == null)
            {
                throw new ArgumentNullException(GetName(expression));
            }
        }

        private static string GetName<T>(Expression<Func<T>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }
    }
}
