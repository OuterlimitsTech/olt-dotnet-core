using System.Collections.Generic;
using System.Linq;

namespace OLT.Email
{
    public class OltEmailRecipientResult
    {
        public virtual List<OltEmailAddressResult> To { get; set; } = new List<OltEmailAddressResult>();
        public virtual List<OltEmailAddressResult> CarbonCopy { get; set; } = new List<OltEmailAddressResult>();
    }
}