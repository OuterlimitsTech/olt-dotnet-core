using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Models;

namespace OLT.EF.Core.Services.Tests.Automapper.Maps;

public class NameAutoMapperModelAfterMap : OltAdapterAfterMap<UserEntity, NameModel>
{
    public override IQueryable<NameModel> AfterMap(IQueryable<NameModel> queryable)
    {
        return queryable.OrderBy(p => p.Last).ThenBy(p => p.First);
    }
}
