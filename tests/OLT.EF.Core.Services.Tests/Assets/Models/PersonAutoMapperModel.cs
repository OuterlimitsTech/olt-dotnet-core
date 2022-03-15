using AutoMapper;
using OLT.EF.Core.Services.Tests.Assets.Entites;

namespace OLT.EF.Core.Services.Tests.Assets.Models
{
    public class PersonAutoMapperModel
    {
        public int? PersonId { get; set; }
        public NameAutoMapperModel Name { get; set; } = new NameAutoMapperModel();

        public static IMappingExpression<PersonEntity, PersonAutoMapperModel> BuildMap(IMappingExpression<PersonEntity, PersonAutoMapperModel> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.PersonId, opt => opt.MapFrom(t => t.Id))
                .ForMember(f => f.Name, opt => opt.MapFrom(t => t))
                .ReverseMap();

            return mappingExpression;
        }
    }
}
