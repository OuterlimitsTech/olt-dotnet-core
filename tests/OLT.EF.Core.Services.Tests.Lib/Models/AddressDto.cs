namespace OLT.EF.Core.Services.Tests.Assets.Models
{
    public class AddressDto
    {
        public int? AddressId { get; set; }
        public int PersonId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }

        public static AddressDto FakerDto()
        {
            return new AddressDto
            {
                Street = Faker.Address.StreetAddress(),
                City = Faker.Address.City(),
            };
        }

        //public static IMappingExpression<AddressEntity, AddressDto> BuildMap(IMappingExpression<AddressEntity, AddressDto> mappingExpression)
        //{
        //    mappingExpression
        //        .ForMember(f => f.AddressId, opt => opt.MapFrom(t => t.Id))
        //        .ForMember(f => f.PersonId, opt => opt.MapFrom(t => t.PersonId))
        //        .ForMember(f => f.Street, opt => opt.MapFrom(t => t.Street))
        //        .ForMember(f => f.City, opt => opt.MapFrom(t => t.City))
        //        .ReverseMap()
        //        .EqualityComparison((model, entity) => model.AddressId == entity.Id)
        //        //.ForMember(f => f.Id, opt => opt.Ignore())
        //        ;

        //    return mappingExpression;
        //}
    }
}
