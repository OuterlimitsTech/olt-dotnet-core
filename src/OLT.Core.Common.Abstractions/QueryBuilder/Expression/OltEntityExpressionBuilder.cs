using System.Linq;

namespace OLT.Core
{

    public abstract class OltEntityExpressionBuilder<TEntity, TValueType> : IOltEntityQueryBuilder<TEntity>
        where TEntity : class, IOltEntity
        where TValueType: notnull
    {
        public virtual TValueType Value { get; set; } = default!;
        public abstract IQueryable<TEntity> BuildQueryable(IQueryable<TEntity> queryable);
    }
}