using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System;
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

    public class AdapterObject3ToAdapterObject5AfterMap : OltAdapterAfterMap<AdapterObject3, AdapterObject5>
    {
        private readonly string _value;

        public AdapterObject3ToAdapterObject5AfterMap(string value)
        {
            _value = value;
        }

        
        public override IQueryable<AdapterObject5> AfterMap(IQueryable<AdapterObject5> queryable)
        {
            return queryable.Where(p => p.FirstName.Contains(_value));
        }
    }

    public class AdapterObject3ToAdapterObject1QueryableAdapter : OltAdapter<AdapterObject3, AdapterObject1>, IOltAdapterQueryable<AdapterObject3, AdapterObject1>
    {
        public override void Map(AdapterObject1 source, AdapterObject3 destination)
        {
            throw new NotImplementedException();
        }

        public override void Map(AdapterObject3 source, AdapterObject1 destination)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AdapterObject1> Map(IQueryable<AdapterObject3> queryable)
        {
            return queryable.Select(entity => new AdapterObject1
            {
                FirstName = entity.First,
                LastName = entity.Last,
            });
        }
    }
}
