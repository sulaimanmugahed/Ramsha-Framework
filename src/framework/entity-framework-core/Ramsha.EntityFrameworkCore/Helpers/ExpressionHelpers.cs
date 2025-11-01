using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ramsha.EntityFrameworkCore;

public static class ExpressionHelpers
{
    public enum CombineOperator { And, Or }

    public static Expression<Func<T, bool>> CombineExpressions<T>(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right,
        CombineOperator op = CombineOperator.Or)
    {
        var param = Expression.Parameter(typeof(T), "x");

        var leftBody = ReplaceParameter(left.Body, left.Parameters[0], param);
        var rightBody = ReplaceParameter(right.Body, right.Parameters[0], param);

        Expression body = op switch
        {
            CombineOperator.And => Expression.AndAlso(leftBody, rightBody),
            CombineOperator.Or => Expression.OrElse(leftBody, rightBody),
            _ => throw new ArgumentOutOfRangeException(nameof(op))
        };

        return Expression.Lambda<Func<T, bool>>(body, param);
    }


    public static LambdaExpression ConvertToEntityType(LambdaExpression interfaceLambda, Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");
        var body = ReplaceParameter(interfaceLambda.Body, interfaceLambda.Parameters[0], parameter);

        return Expression.Lambda(body!, parameter);
    }

    public static Expression ReplaceParameter(Expression body, ParameterExpression from, ParameterExpression to)
    {
        return new ParameterReplaceVisitor(from, to).Visit(body)!;
    }

    private class ParameterReplaceVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _from;
        private readonly ParameterExpression _to;

        public ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to)
        {
            _from = from;
            _to = to;
        }

        protected override Expression VisitParameter(ParameterExpression node)
            => node == _from ? _to : base.VisitParameter(node);
    }
}

