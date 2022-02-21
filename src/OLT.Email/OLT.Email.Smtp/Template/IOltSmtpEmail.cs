using System.Collections.Generic;

namespace OLT.Email.Smtp
{
    public interface IOltSmtpEmail 
    {
        string Subject { get; }
        string Body { get; }
        IOltEmailAddress From { get; }
        OltEmailRecipients Recipients { get; }
    }
}