using AutoMapper;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets.Models
{

    public class NameAutoMapperModel : OltPersonName
    {
        public static IMappingExpression<PersonEntity, NameAutoMapperModel> BuildMap(IMappingExpression<PersonEntity, NameAutoMapperModel> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.First, opt => opt.MapFrom(t => t.NameFirst))
                .ForMember(f => f.Middle, opt => opt.MapFrom(t => t.NameMiddle))
                .ForMember(f => f.Last, opt => opt.MapFrom(t => t.NameLast))
                .ReverseMap();

            return mappingExpression;
        }

        public static IMappingExpression<UserEntity, NameAutoMapperModel> BuildMap(IMappingExpression<UserEntity, NameAutoMapperModel> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.First, opt => opt.MapFrom(t => t.FirstName))
                .ForMember(f => f.Middle, opt => opt.MapFrom(t => t.MiddleName))
                .ForMember(f => f.Last, opt => opt.MapFrom(t => t.LastName))
                .ForMember(f => f.Suffix, opt => opt.MapFrom(t => t.NameSuffix))
                .ReverseMap();

            return mappingExpression;
        }


        public static NameAutoMapperModel FakerEntity()
        {
            return new NameAutoMapperModel
            {
                First = Faker.Name.First(),
                Middle = Faker.Name.Middle(),
                Last = Faker.Name.Last(),
            };
        }

    }
}
