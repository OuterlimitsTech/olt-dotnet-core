using System.Collections.Generic;

namespace OLT.Email.Smtp
{
    public class OltApplicationErrorEmail : OltSmtpEmail
    {
        public virtual string AppName { get; set; }
        public virtual string Environment { get; set; }
    }
}