using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System.Linq;

namespace OLT.DataAdapters.Tests.PagedAdapterTests.Adapters
{
    public class AdapterObject3ToAdapterObject1Adapter : OltAdapter<AdapterObject3, AdapterObject1>, IOltAdapterQueryable<AdapterObject3, AdapterObject1>
    {
        public AdapterObject3ToAdapterObject1Adapter()
        {
            OltAdapterMapConfigs.AfterMap.Register(new OltAfterMapOrderBy<AdapterObject3, AdapterObject1>(p => p.OrderBy(p => p.LastName).ThenBy(p => p.FirstName)));
        }

        public override void Map(AdapterObject1 source, AdapterObject3 destination)
        {
            destination.First = source.FirstName;
            destination.Last = source.LastName;

        }

        public override void Map(AdapterObject3 source, AdapterObject1 destination)
        {
            destination.FirstName = source.First;
            destination.LastName = source.Last;
        }

        public IQueryable<AdapterObject1> Map(IQueryable<AdapterObject3> queryable)
        {
            return queryable.Select(entity => new AdapterObject1 { FirstName = entity.First, LastName = entity.Last });
        }
    }
}