using OLT.Core;

namespace OLT.Email
{
    public class OltEmailAddressResult : OltEmailAddress
    {
        public OltEmailAddressResult(IOltEmailAddress copyFrom, OltEmailBuilderArgs args)
        {
            Name = copyFrom.Name;
            Email = copyFrom.Email;

            if (!args.AllowSend(Email))
            {
                Skipped = true;
                SkipReason = "Email not in whitelist";
            }
        }

        public bool Success => Sent;
        public virtual bool Sent => !Skipped && string.IsNullOrWhiteSpace(Error);

        public virtual bool Skipped { get; set; }
        public virtual string SkipReason { get; set; }
        public virtual string Error { get; set; }
        
    }
}