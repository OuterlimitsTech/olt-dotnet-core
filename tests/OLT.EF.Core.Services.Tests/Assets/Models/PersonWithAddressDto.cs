using AutoMapper;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;
using System.Collections.Generic;

namespace OLT.EF.Core.Services.Tests.Assets.Models
{
    public class PersonWithAddressDto : PersonDto
    {

        public List<AddressDto> Addresses = new List<AddressDto>();

        public static PersonWithAddressDto FakerDto(int numberAddresses)
        {
            var dto = new PersonWithAddressDto
            {
                UniqueId = Guid.NewGuid(),
                First = Faker.Name.First(),
                Middle = Faker.Name.Middle(),
                Last = Faker.Name.Last(),
            };

            for(var idx = 1; idx <= numberAddresses; idx++)
            {
                dto.Addresses.Add(AddressDto.FakerDto());
            }

            return dto;
        }


        //public static IMappingExpression<PersonEntity, PersonWithAddressDto> BuildMap(IMappingExpression<PersonEntity, PersonWithAddressDto> mappingExpression)
        //{
        //    mappingExpression
        //        .ForMember(f => f.PersonId, opt => opt.MapFrom(t => t.Id))
        //        .ForMember(f => f.UniqueId, opt => opt.MapFrom(t => t.UniqueId))
        //        .ForMember(f => f.First, opt => opt.MapFrom(t => t.NameFirst))
        //        .ForMember(f => f.Middle, opt => opt.MapFrom(t => t.NameMiddle))
        //        .ForMember(f => f.Last, opt => opt.MapFrom(t => t.NameLast))
        //        .ForMember(f => f.Addresses, opt => opt.MapFrom(t => t.Addresses))
        //        .ReverseMap()
        //        .ForMember(f => f.Id, opt => opt.Ignore())
        //        ;

        //    return mappingExpression;
        //}
    }
}
