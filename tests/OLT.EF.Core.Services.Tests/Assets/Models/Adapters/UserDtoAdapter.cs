﻿using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;

namespace OLT.EF.Core.Services.Tests.Assets.Models.Adapters
{
    public class UserDtoAdapter : OltAdapter<UserEntity, UserDto>
    {
        public override void Map(UserEntity source, UserDto destination)
        {
            destination.UserId = source.Id;
            destination.First = source.FirstName;
            destination.Middle = source.MiddleName;
            destination.Last = source.LastName;
            destination.Suffix = source.NameSuffix;

        }

        public override void Map(UserDto source, UserEntity destination)
        {
            destination.FirstName = source.First;
            destination.MiddleName = source.Middle;
            destination.LastName = source.Last;
            destination.NameSuffix = source.Suffix;
        }

    }
}
