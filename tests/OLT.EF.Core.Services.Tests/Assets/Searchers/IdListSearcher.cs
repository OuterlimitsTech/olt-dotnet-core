using OLT.Core;
using System.Collections.Generic;
using System.Linq;

namespace OLT.EF.Core.Services.Tests.Assets.Searchers
{
    public class IdListSearcher<TEntity> : OltSearcher<TEntity>
        where TEntity : class, IOltEntityId
    {
        public IdListSearcher(params int[] ids)
        {
            Ids = ids.ToList();
        }

        public List<int> Ids { get; }

        public override IQueryable<TEntity> BuildQueryable(IQueryable<TEntity> queryable)
        {
            return queryable.Where(p => Ids.Contains(p.Id));
        }
    }
}
