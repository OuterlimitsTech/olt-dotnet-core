using AutoMapper;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System.Linq;

namespace OLT.DataAdapters.AutoMapper.Tests.Adapters
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class AdapterObject4PagedMap : OltAdapterPagedMap<AdapterObject2, AdapterObject4>
#pragma warning restore CS0618 // Type or member is obsolete
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

#pragma warning disable CS0618 // Type or member is obsolete
    public class AdapterObject2ToAdapterObject5PagedAdapter : OltAdapterPaged<AdapterObject2, AdapterObject5>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        public override void Map(AdapterObject2 source, AdapterObject5 destination)
        {
            throw new System.NotImplementedException();
        }

        public override void Map(AdapterObject5 source, AdapterObject2 destination)
        {
            throw new System.NotImplementedException();
        }

        //public override void Map(AdapterObject5 source, AdapterObject2 destination)
        //{
        //    destination.Name = new OltPersonName
        //    {
        //        First = source.First,
        //        Last = source.Last,
        //    };
        //}

        //public override void Map(AdapterObject2 source, AdapterObject5 destination)
        //{
        //    destination.First = source.Name.First;
        //    destination.Last = source.Name.Last;
        //}

        public override IQueryable<AdapterObject5> Map(IQueryable<AdapterObject2> queryable)
        {
            return queryable.Select(entity => new AdapterObject5
            {
                ObjectId = entity.ObjectId,
                First = entity.Name.First, 
                Last = entity.Name.Last,
            });
        }

        public override IQueryable<AdapterObject2> DefaultOrderBy(IQueryable<AdapterObject2> queryable)
        {
            return queryable.OrderBy(p => p.Name.Last).ThenBy(p => p.Name.First).ThenBy(p => p.ObjectId);
        }

  
    }
}
