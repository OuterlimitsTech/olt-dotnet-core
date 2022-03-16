using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System.Linq;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{
    public interface IUserService : IOltEntityService<UserEntity>
    {
        IQueryable<UserEntity> GetRepository();
    }
}
