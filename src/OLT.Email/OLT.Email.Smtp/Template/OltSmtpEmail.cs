using System.Collections.Generic;

namespace OLT.Email.Smtp
{
    public class OltSmtpEmail : IOltSmtpEmail
    {
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual IOltEmailAddress From { get; set; } = new OltEmailAddress();
        public OltEmailRecipients Recipients { get; set; } = new OltEmailRecipients();
    }
}