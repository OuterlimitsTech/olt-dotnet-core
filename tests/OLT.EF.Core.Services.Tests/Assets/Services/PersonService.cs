using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{
    public interface IPersonService : IOltEntityIdService<PersonEntity>
    {
    }

    public interface IPersonUniqueIdService : IOltEntityUniqueIdService<PersonEntity>
    {
    }

    public class PersonService : OltEntityIdService<UnitTestContext, PersonEntity>, IPersonService
    {
        public PersonService(
            IOltServiceManager serviceManager,
            UnitTestContext context) : base(serviceManager, context)
        {
        }
    }

    public class PersonUniqueIdService : OltEntityUniqueIdService<UnitTestContext, PersonEntity>, IPersonUniqueIdService
    {
        public PersonUniqueIdService(
            IOltServiceManager serviceManager,
            UnitTestContext context) : base(serviceManager, context)
        {
        }
    }
}
