using System.Linq.Expressions;

namespace OLT.Core
{
    public class OltEntityExpressionIntContains<TEntity> : IOltEntityQueryBuilder<TEntity, List<int>>
        where TEntity : class, IOltEntity
    {
        public OltEntityExpressionIntContains(Expression<Func<TEntity, int>> fieldExpression)
        {
            FieldExpression = fieldExpression;
        }

        public List<int> Value { get; set; } = new List<int>();

        private Expression<Func<TEntity, int>> FieldExpression { get; }

        private Expression<Func<int, bool>> WhereExpression
        {
            get
            {
                return (value) => Value.Contains(value);
            }
        }

        public IQueryable<TEntity> BuildQueryable(IQueryable<TEntity> queryable)
        {
            return queryable.Where(OltPredicateBuilder.BuildExpression(FieldExpression, WhereExpression));
        }
    }
}