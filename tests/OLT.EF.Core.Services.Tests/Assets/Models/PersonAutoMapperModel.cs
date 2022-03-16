using AutoMapper;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;
using System.Collections.Generic;

namespace OLT.EF.Core.Services.Tests.Assets.Models
{

    public class PersonAutoMapperModel
    {
        public int? PersonId { get; set; }
        public Guid? UniqueId { get; set; }
        public NameAutoMapperModel Name { get; set; } = new NameAutoMapperModel();

        public static IMappingExpression<PersonEntity, PersonAutoMapperModel> BuildMap(IMappingExpression<PersonEntity, PersonAutoMapperModel> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.PersonId, opt => opt.MapFrom(t => t.Id))
                .ForMember(f => f.UniqueId, opt => opt.MapFrom(t => t.UniqueId))
                .ForMember(f => f.Name, opt => opt.MapFrom(t => t))
                .ReverseMap();

            return mappingExpression;
        }


        public static PersonAutoMapperModel FakerEntity()
        {
            return new PersonAutoMapperModel
            {
                UniqueId = Guid.NewGuid(),
                Name = NameAutoMapperModel.FakerEntity(),
            };
        }

        public static List<PersonAutoMapperModel> FakerList(int number)
        {
            var list = new List<PersonAutoMapperModel>();

            for (int i = 0; i < number; i++)
            {
                list.Add(FakerEntity());
            }

            return list;
        }
    }
}
