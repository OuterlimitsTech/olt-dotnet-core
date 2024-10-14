using System.Linq.Expressions;

namespace OLT.Core
{
    public static class OltGeneralQueryableExtensions
    {
        public static IQueryable<TEntity> NonDeletedQueryable<TEntity>(this IQueryable<TEntity> queryable)
            where TEntity : class, IOltEntity
        {
            if (!typeof(IOltEntityDeletable).IsAssignableFrom(typeof(TEntity))) return queryable;
            Expression<Func<TEntity, bool>> getNonDeleted = deletableQuery => ((IOltEntityDeletable)deletableQuery).DeletedOn == null;
            getNonDeleted = (Expression<Func<TEntity, bool>>)OltRemoveCastsVisitor.Visit(getNonDeleted);
            return queryable.Where(getNonDeleted);
        }
    }
}
