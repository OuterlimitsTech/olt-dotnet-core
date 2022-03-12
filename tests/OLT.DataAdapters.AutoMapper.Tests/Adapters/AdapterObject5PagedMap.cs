using AutoMapper;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System.Linq;

namespace OLT.DataAdapters.AutoMapper.Tests.Adapters
{
    public class AdapterObject5PagedMap : OltAdapterPagedMap<AdapterObject2, AdapterObject5>
    {
        public override void BuildMap(IMappingExpression<AdapterObject2, AdapterObject5> mappingExpression)
        {
            throw new System.NotImplementedException();
        }

        public override IQueryable<AdapterObject2> DefaultOrderBy(IQueryable<AdapterObject2> queryable)
        {
            throw new System.NotImplementedException();
        }
    }
}
