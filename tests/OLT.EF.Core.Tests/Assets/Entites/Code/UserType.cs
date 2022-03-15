using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Tests.Assets.Entites.Code
{
    public class UserType : BaseCodeValueEntity, IOltEntityUniqueId
    {
        public Guid UniqueId { get; set; }
    }
}
