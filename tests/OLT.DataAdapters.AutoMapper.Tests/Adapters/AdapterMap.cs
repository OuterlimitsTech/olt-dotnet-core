using AutoMapper;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System.Linq;

namespace OLT.DataAdapters.AutoMapper.Tests.AdapterTests
{
    public class AdapterMap : Profile
    {
        public AdapterMap()
        {
            BuildMap(CreateMap<AdapterObject1, AdapterObject2>());
            BuildMap(CreateMap<AdapterObject1, OltPersonName>());
            BuildMap(CreateMap<AdapterObject3, OltPersonName>());
        }

        public void BuildMap(IMappingExpression<AdapterObject1, AdapterObject2> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.ObjectId, opt => opt.MapFrom(t => t.ObjectId))
                .ForMember(f => f.Name, opt => opt.MapFrom(t => t))
                .ReverseMap()                
                ;
        }

        public void BuildMap(IMappingExpression<AdapterObject1, OltPersonName> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.First, opt => opt.MapFrom(t => t.FirstName))
                .ForMember(f => f.Last, opt => opt.MapFrom(t => t.LastName))
                .ReverseMap()
                ;
        }


        public void BuildMap(IMappingExpression<AdapterObject3, OltPersonName> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.First, opt => opt.MapFrom(t => t.First))
                .ForMember(f => f.Last, opt => opt.MapFrom(t => t.Last))
                .ReverseMap()
                ;
        }

        public void BuildMap(IMappingExpression<AdapterObject2, AdapterObject3> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.ObjectId, opt => opt.MapFrom(t => t.ObjectId))
                .ForMember(f => f, opt => opt.MapFrom(t => t.Name))
                .ReverseMap()
                ;
        }

    }
}
