using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System.Linq;

namespace OLT.EF.Core.Services.Tests.Assets.Models.Maps
{
    public class NameAutoMapperModelAfterMap : OltAdapterAfterMap<UserEntity, NameAutoMapperModel>
    {
        public override IQueryable<NameAutoMapperModel> AfterMap(IQueryable<NameAutoMapperModel> queryable)
        {
            return queryable.OrderBy(p => p.Last).ThenBy(p => p.First);
        }
    }
}
