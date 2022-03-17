using OLT.Core;
using OLT.DataAdapters.Tests.ProjectToTests.Models;
using System.Linq;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{

    /// <summary>
    /// Does nothing for testing 
    /// </summary>
    public class AdapterObject3ToAdapterObject5AfterMap : OltAdapterAfterMap<QueryableAdapterObject3, QueryableAdapterObject5>
    {

        public AdapterObject3ToAdapterObject5AfterMap()
        {
        }


        public override IQueryable<QueryableAdapterObject5> AfterMap(IQueryable<QueryableAdapterObject5> queryable)
        {
            return queryable;
        }
    }
}
