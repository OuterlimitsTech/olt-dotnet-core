using OLT.Core;
using OLT.DataAdapters.Tests.ProjectToTests.Models;
using System.Linq;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{

    /// <summary>
    /// Does nothing - for testing
    /// </summary>
    public class AdapterObject3ToAdapterObject4BeforeMap : OltAdapterBeforeMap<QueryableAdapterObject3, QueryableAdapterObject4>
    {

        public AdapterObject3ToAdapterObject4BeforeMap()
        {
        }


        public override IQueryable<QueryableAdapterObject3> BeforeMap(IQueryable<QueryableAdapterObject3> queryable)
        {
            return queryable;
        }
    }
}
