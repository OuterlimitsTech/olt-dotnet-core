using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System.Linq;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{

    /// <summary>
    /// Does nothing - for testing
    /// </summary>
    public class AdapterObject3ToAdapterObject4BeforeMap : OltAdapterBeforeMap<AdapterObject3, AdapterObject4>
    {

        public AdapterObject3ToAdapterObject4BeforeMap()
        {
        }


        public override IQueryable<AdapterObject3> BeforeMap(IQueryable<AdapterObject3> queryable)
        {
            return queryable;
        }
    }
}
