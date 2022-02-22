using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email.Common.Tests.Assets
{
    public class TestArgs : OltFromEmailArgs<TestArgs>
    {

        public OltEmailAddress EmailValue => base.From;
        public List<OltEmailAddress> ToValue => base.To;
        public List<OltEmailAddress> CarbonCopyValue => base.CarbonCopy;
        public OltEmailRecipientResult BuildRecipientsValue => base.BuildRecipients();
        public List<OltEmailAttachment> AttachmentValue => base.Attachments;

        public bool DoValidation()
        {
            var errors = Validate();
            if (errors.Any())
            {
                throw new TestException(errors);
            }
            return true;
        }

        public List<string> GetErrors()
        {
            return Validate();
        }
    }
}
