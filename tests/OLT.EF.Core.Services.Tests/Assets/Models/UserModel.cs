using AutoMapper;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;

namespace OLT.EF.Core.Services.Tests.Assets.Models
{
    public class UserModel
    {
        public int? UserId { get; set; }
        public Guid UserGuid { get; set; }
        public NameAutoMapperModel Name { get; set; } = new NameAutoMapperModel();

        public static UserModel FakerEntity()
        {
            return new UserModel
            {
                UserGuid = Guid.NewGuid(),
                Name = NameAutoMapperModel.FakerEntity(),
            };
        }
    }
}
