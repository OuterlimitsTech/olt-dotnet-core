using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System.Linq;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{

    /// <summary>
    /// Does nothing for testing 
    /// </summary>
    public class AdapterObject3ToAdapterObject5AfterMap : OltAdapterAfterMap<AdapterObject3, AdapterObject5>
    {

        public AdapterObject3ToAdapterObject5AfterMap()
        {
        }

        
        public override IQueryable<AdapterObject5> AfterMap(IQueryable<AdapterObject5> queryable)
        {
            return queryable;
        }
    }
}
