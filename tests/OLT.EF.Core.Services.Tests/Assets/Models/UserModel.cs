using AutoMapper;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;

namespace OLT.EF.Core.Services.Tests.Assets.Models
{
    public class UserModel
    {
        public int? UserId { get; set; }
        public Guid UserGuid { get; set; }
        public NameAutoMapperModel Name { get; set; } = new NameAutoMapperModel();

        //public static IMappingExpression<UserEntity, UserModel> BuildMap(IMappingExpression<UserEntity, UserModel> mappingExpression)
        //{
        //    mappingExpression
        //        .ForMember(f => f.UserId, opt => opt.MapFrom(t => t.Id))
        //        .ForMember(f => f.Name, opt => opt.MapFrom(t => t))
        //        .ReverseMap();

        //    return mappingExpression;
        //}


        public static UserModel FakerEntity()
        {
            return new UserModel
            {
                UserGuid = Guid.NewGuid(),
                Name = NameAutoMapperModel.FakerEntity(),
            };
        }
    }

    public class UserDto : OltPersonName
    {
        public int? UserId { get; set; }
        public Guid UserGuid { get; set; }

        public static UserDto FakerEntity()
        {
            return new UserDto
            {
                First = Faker.Name.First(),
                Middle = Faker.Name.Middle(),
                Last = Faker.Name.Last(),
            };
        }
    }
}
