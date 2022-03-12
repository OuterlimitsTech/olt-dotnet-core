using AutoMapper;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System.Linq;

namespace OLT.DataAdapters.AutoMapper.Tests.Adapters
{
    public class AdapterObject4PagedMap : OltAdapterPagedMap<AdapterObject2, AdapterObject4>
    {
        public bool IsDisposed()
        {
            return base.Disposed;
        }

        public override void BuildMap(IMappingExpression<AdapterObject2, AdapterObject4> mappingExpression)
        {
            BuildNameMap(CreateMap<AdapterObject4, OltPersonName>());

            mappingExpression
                .ForMember(f => f.ObjectId, opt => opt.MapFrom(t => t.ObjectId))
                .ForMember(f => f.Name, opt => opt.MapFrom(t => t.Name));            
        }

        public override IQueryable<AdapterObject2> DefaultOrderBy(IQueryable<AdapterObject2> queryable)
        {
            return queryable.OrderBy(p => p.Name.Last).ThenBy(p => p.Name.First).ThenBy(p => p.ObjectId);
        }

        public void BuildNameMap(IMappingExpression<AdapterObject4, OltPersonName> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.First, opt => opt.MapFrom(t => t.Name.First))
                .ForMember(f => f.Last, opt => opt.MapFrom(t => t.Name.Last))
                .ReverseMap()
                ;
        }

    }
}
