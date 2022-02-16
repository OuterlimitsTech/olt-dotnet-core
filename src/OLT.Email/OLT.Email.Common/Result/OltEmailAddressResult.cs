using OLT.Core;

namespace OLT.Email
{
    public class OltEmailAddressResult : OltEmailAddress
    {
        public OltEmailAddressResult(IOltEmailAddress copyFrom, OltEmailConfiguration configuration)
        {
            Name = copyFrom.Name;
            Email = copyFrom.Email;

            if (!configuration.Production && !configuration.SendEmail(Email))
            {
                Skipped = true;
                SkipReason = "Email not in configuration to send";
            }
        }

        public bool Success => Sent;
        public virtual bool Sent => !Skipped && string.IsNullOrWhiteSpace(Error);

        public virtual bool Skipped { get; set; }
        public virtual string SkipReason { get; set; }
        public virtual string Error { get; set; }
        
    }
}