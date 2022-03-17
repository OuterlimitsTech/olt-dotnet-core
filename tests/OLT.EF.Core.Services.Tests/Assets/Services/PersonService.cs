using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{

    public class PersonService : OltEntityIdService<UnitTestContext, PersonEntity>, IPersonService
    {
        public PersonService(
            IOltServiceManager serviceManager,
            UnitTestContext context) : base(serviceManager, context)
        {
        }
    }
}
