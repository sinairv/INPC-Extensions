using System;
using System.Linq.Expressions;

namespace NotifyPropertyChangedExtensions
{
    public static class ExpressionExtensions
    {
        public static bool IsNotOfMemberAccess<TDelegate>(this Expression<TDelegate> expression, out MemberExpression negatedMember)
        {
            if (expression == null) 
                throw new ArgumentNullException("expression");

            var body = expression.Body;

            if (body.NodeType == ExpressionType.Not)
            {
                var unaryExpression = body as UnaryExpression;
                if (unaryExpression != null)
                {
                    var negatedBody = unaryExpression.Operand;
                    if (negatedBody.NodeType == ExpressionType.MemberAccess)
                    {
                        negatedMember = negatedBody as MemberExpression;
                        return negatedMember != null;
                    }
                }
            }

            negatedMember = null;
            return false;
        }

        public static bool IsNotOfMemberAccess<TDelegate>(this Expression<TDelegate> expression)
        {
            MemberExpression memberExpression;
            return IsNotOfMemberAccess(expression, out memberExpression);
        }
    }
}
