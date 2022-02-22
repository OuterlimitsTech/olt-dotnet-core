using System.Collections.Generic;

namespace OLT.Email
{
    public class OltApplicationErrorEmail : OltSmtpEmail
    {
        public virtual string AppName { get; set; }
        public virtual string Environment { get; set; }
    }
}