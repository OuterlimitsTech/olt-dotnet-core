using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System.Linq;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{
    public class UserService : OltEntityService<UnitTestContext, UserEntity>, IUserService
    {
        public UserService(
            IOltServiceManager serviceManager,
            UnitTestContext context) : base(serviceManager, context)
        {
        }

        public IQueryable<UserEntity> GetRepository()
        {
            return Repository;
        }
    }
}
