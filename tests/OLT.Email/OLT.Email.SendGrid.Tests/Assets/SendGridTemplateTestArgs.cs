using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email.SendGrid.Tests.Assets
{
    public class SendGridTemplateTestArgs : OltTemplateArgs<SendGridTemplateTestArgs>
    {
        public IOltEmailTemplate TemplateValue => base.Template;
        public string ApiKeyValue => base.ApiKey;
        public int? UnsubscribeGroupIdValue => base.UnsubscribeGroupId;
        public bool ClickTrackingValue => base.ClickTracking;


        public bool DoValidation()
        {
            var errors = Validate();
            if (errors.Any())
            {
                throw new OltSendGridValidationException(errors);
            }
            return true;
        }

        public List<string> GetErrors()
        {
            return Validate();
        }

        //public override OltEmailResult Send()
        //{
        //    throw new NotImplementedException();
        //}

        //public override Task<OltEmailResult> SendAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
