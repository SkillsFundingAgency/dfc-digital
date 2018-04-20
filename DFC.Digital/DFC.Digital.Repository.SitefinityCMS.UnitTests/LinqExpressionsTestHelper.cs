using System;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public static class LinqExpressionsTestHelper
    {
        public static bool IsExpressionEqual(Expression<Func<DynamicContent, bool>> x, Expression<Func<DynamicContent, bool>> y)
        {
            return ExpressionComparer(x, y);
        }

        private static bool ExpressionComparer(Expression x, Expression y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.NodeType != y.NodeType)
            {
                return false;
            }

            switch (x.NodeType)
            {
                case ExpressionType.Equal:
                case ExpressionType.AndAlso:
                    var hasMatched = ExpressionComparer(((BinaryExpression)x).Left, ((BinaryExpression)y).Left)
                        && ExpressionComparer(((BinaryExpression)x).Right, ((BinaryExpression)y).Right);
                    return hasMatched;

                case ExpressionType.Lambda:
                    var hasMatched2 = ExpressionComparer(((LambdaExpression)x).Body, ((LambdaExpression)y).Body);
                    return hasMatched2;

                case ExpressionType.Convert:
                    var hasMatched3 = ExpressionComparer(((UnaryExpression)x).Operand, ((UnaryExpression)y).Operand);
                    return hasMatched3;

                case ExpressionType.MemberAccess:
                    MemberExpression mex = (MemberExpression)x;
                    MemberExpression mey = (MemberExpression)y;
                    var hasMatched4 = mex.Member == mey.Member;
                    if (!hasMatched4)
                    {
                        var left = Expression.Lambda<Func<string>>(x).Compile().Invoke();
                        var right = Expression.Lambda<Func<string>>(y).Compile().Invoke();
                        hasMatched4 = left == right;
                    }

                    return hasMatched4;

                case ExpressionType.Constant:
                    var hasMatched5 = ((ConstantExpression)x).Value.ToString() == ((ConstantExpression)y).Value.ToString();
                    return hasMatched5;

                case ExpressionType.Conditional:
                    var hasMatched7 = ExpressionComparer(((ConditionalExpression)x).IfTrue, ((ConditionalExpression)y).IfTrue)
                                     && ExpressionComparer(((ConditionalExpression)x).IfFalse, ((ConditionalExpression)y).IfFalse);
                    return hasMatched7;

                default:
                    throw new NotImplementedException($"{x.NodeType}-{x.GetType().Name}-{x.Type.Name}-{x}");
            }
        }
    }
}
