using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System.Linq;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{
    public class AdapterObject3ToAdapterObject4BeforeMap : OltAdapterBeforeMap<AdapterObject3, AdapterObject4>
    {
        private readonly string _value;

        public AdapterObject3ToAdapterObject4BeforeMap(string value)
        {
            _value = value;
        }


        public override IQueryable<AdapterObject3> BeforeMap(IQueryable<AdapterObject3> queryable)
        {
            return queryable.Where(p => p.Last.Contains(_value));
        }
    }
}
