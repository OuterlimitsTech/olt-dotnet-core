using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets.Models.Adapters
{

    public class UserModelAdapter : OltAdapterPaged<UserEntity, UserModel>
    {
        public override void Map(UserEntity source, UserModel destination)
        {
            destination.UserId = source.Id;
            destination.UserGuid = source.UniqueId;
            destination.Name.First = source.FirstName;
            destination.Name.Middle = source.MiddleName;
            destination.Name.Last = source.LastName;
            destination.Name.Suffix = source.NameSuffix;
        }

        public override void Map(UserModel source, UserEntity destination)
        {
            destination.UniqueId = source.UserGuid;
            destination.FirstName = source.Name.First;
            destination.MiddleName = source.Name.Middle;
            destination.LastName = source.Name.Last;
            destination.NameSuffix = source.Name.Suffix;
        }

        public override IQueryable<UserModel> Map(IQueryable<UserEntity> queryable)
        {
            return queryable.Select(entity => new UserModel
            {
                UserId = entity.Id,
                UserGuid = entity.UniqueId,
                Name = new NameAutoMapperModel
                {
                    First = entity.FirstName,
                    Middle = entity.MiddleName,
                    Last = entity.LastName,
                    Suffix = entity.NameSuffix
                }
            });
        }

        public override IQueryable<UserEntity> DefaultOrderBy(IQueryable<UserEntity> queryable)
        {
            return queryable.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ThenBy(p => p.Id);
        }
    }
}
