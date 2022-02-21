using System.Collections.Generic;

namespace OLT.Email.Smtp
{
    public interface IOltApplicationErrorEmail : IOltSmtpEmail
    {
        string AppName { get; } 
        string Environment { get; }
    }
}