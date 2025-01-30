using AutoMapper;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Models;
using OLT.EF.Core.Services.Tests.Automapper.Models;

namespace OLT.EF.Core.Services.Tests.Automapper.Maps;

public class AutomapperMaps : Profile
{
    public AutomapperMaps()
    {
        BuildMap(CreateMap<PersonEntity, PersonModel>()).WithOrderBy(p => p.OrderBy(o => o.NameLast).ThenBy(o => o.NameFirst));
        BuildMap(CreateMap<PersonEntity, NameModel>()).WithOrderBy(p => p.OrderBy(o => o.NameLast).ThenBy(o => o.NameFirst));
        BuildMap(CreateMap<UserEntity, NameModel>()).AfterMap(new NameAutoMapperModelAfterMap());
        BuildMap(CreateMap<PersonEntity, PersonDetailDto>()).WithOrderBy(p => p.OrderBy(o => o.NameLast).ThenBy(o => o.NameFirst).ThenBy(p => p.Id));
    }

    public static IMappingExpression<PersonEntity, PersonModel> BuildMap(IMappingExpression<PersonEntity, PersonModel> mappingExpression)
    {
        mappingExpression
            .ForMember(f => f.PersonId, opt => opt.MapFrom(t => t.Id))
            .ForMember(f => f.UniqueId, opt => opt.MapFrom(t => t.UniqueId))
            .ForMember(f => f.Name, opt => opt.MapFrom(t => t))
            .ReverseMap();

        return mappingExpression;
    }

    public static IMappingExpression<PersonEntity, NameModel> BuildMap(IMappingExpression<PersonEntity, NameModel> mappingExpression)
    {
        mappingExpression
            .ForMember(f => f.First, opt => opt.MapFrom(t => t.NameFirst))
            .ForMember(f => f.Middle, opt => opt.MapFrom(t => t.NameMiddle))
            .ForMember(f => f.Last, opt => opt.MapFrom(t => t.NameLast))
            .ReverseMap();

        return mappingExpression;
    }

    public static IMappingExpression<UserEntity, NameModel> BuildMap(IMappingExpression<UserEntity, NameModel> mappingExpression)
    {
        mappingExpression
            .ForMember(f => f.First, opt => opt.MapFrom(t => t.FirstName))
            .ForMember(f => f.Middle, opt => opt.MapFrom(t => t.MiddleName))
            .ForMember(f => f.Last, opt => opt.MapFrom(t => t.LastName))
            .ForMember(f => f.Suffix, opt => opt.MapFrom(t => t.NameSuffix))
            .ReverseMap();

        return mappingExpression;
    }

    public static IMappingExpression<PersonEntity, PersonDetailDto> BuildMap(IMappingExpression<PersonEntity, PersonDetailDto> mappingExpression)
    {
        mappingExpression
            .ForMember(f => f.PersonId, opt => opt.MapFrom(t => t.Id))
            .ForMember(f => f.First, opt => opt.MapFrom(t => t.NameFirst))
            .ForMember(f => f.Middle, opt => opt.MapFrom(t => t.NameMiddle))
            .ForMember(f => f.Last, opt => opt.MapFrom(t => t.NameLast))
            .ForMember(f => f.Created, opt => opt.MapFrom(t => t.CreateDate))
            .ReverseMap()
            .ForMember(f => f.Id, opt => opt.Ignore())
            .ForMember(f => f.CreateDate, opt => opt.Ignore())
            ;

        return mappingExpression;
    }
}
