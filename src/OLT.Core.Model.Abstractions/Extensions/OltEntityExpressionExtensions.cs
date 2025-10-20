using System.Linq.Expressions;

namespace OLT.Core;

public static class OltEntityExpressionExtensions
{
    /// <summary>
    /// Builds Queryable Expression using provided Query Expression and Field Expression within Dynamic Filter
    /// </summary>
    /// <typeparam name="TEntity">EF Entity -> determined by provided expressions</typeparam>
    /// <typeparam name="TValueType">Field Type -> determined by provided expressions</typeparam>
    /// <param name="dynamicFilter">Dynamic Filter Object</param>
    /// <returns></returns>
    public static Expression<Func<TEntity, bool>> BuildExpression<TEntity, TValueType>(this IOltEntityExpression<TEntity, TValueType> dynamicFilter)
        where TEntity : class, IOltEntity
    {
        return BuildExpression(dynamicFilter.FieldExpression, dynamicFilter.WhereExpression);
    }

    /// <summary>
    /// Builds Queryable Expression using provided Query Expression and Field Expression
    /// </summary>
    /// <typeparam name="TEntity">EF Entity -> determined by provided expressions</typeparam>
    /// <typeparam name="TValueType">Field Type -> determined by provided expressions</typeparam>
    /// <param name="queryExpression">Query to build: (value) => value.contains('Test Value')</param>
    /// <param name="fieldExpression">The field to Query: entity => entity.field</param>
    /// <returns></returns>
    public static Expression<Func<TEntity, bool>> BuildExpression<TEntity, TValueType>(Expression<Func<TEntity, TValueType>> fieldExpression, Expression<Func<TValueType, bool>> queryExpression)
        where TEntity : class, IOltEntity
    {
        var entityParam = Expression.Parameter(typeof(TEntity), "entity");
        var fieldValue = fieldExpression.Body.ReplaceParameter(fieldExpression.Parameters[0], entityParam);
        var baseExpr = queryExpression;
        var containsParam = baseExpr.Parameters[0];
        var expr = baseExpr.Body.ReplaceParameter(containsParam, fieldValue);
        return Expression.Lambda<Func<TEntity, bool>>(expr, entityParam);
    }

}
