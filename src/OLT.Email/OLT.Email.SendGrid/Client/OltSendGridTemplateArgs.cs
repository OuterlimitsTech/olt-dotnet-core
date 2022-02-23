using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;

namespace OLT.Email.SendGrid
{

    public class OltSendGridTemplateArgs : OltTemplateArgs<OltSendGridTemplateArgs>
    {

        protected internal virtual SendGridClient CreateClient()
        {
            return new SendGridClient(ApiKey);
        }

        protected internal virtual SendGridMessage BuildMessage()
        {   
            var errors = Validate();
            if (errors.Any())
            {
                throw new OltSendGridValidationException(errors);
            }

            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(From.Email, From.Name));
            msg.SetTemplateId(Template.TemplateId);

            if (ClickTracking)
            {
                msg.SetClickTracking(true, true);
            }

            if (UnsubscribeGroupId.HasValue)
            {
                msg.SetAsm(UnsubscribeGroupId.Value);
            }

            var templateData = Template.GetTemplateData();
            if (templateData != null)
            {
                msg.SetTemplateData(templateData);
            }

            Attachments?.ForEach(attachment => msg.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Bytes), attachment.ContentType));

            var recipients = BuildRecipients();
            ConfigureRecipients(msg, recipients);

            return msg;
        }

        protected virtual void ConfigureRecipients(SendGridMessage msg, OltEmailRecipientResult recipients)
        {
            recipients.To.Where(p => !p.Skipped && string.IsNullOrWhiteSpace(p.Error)).ToList().ForEach(rec =>
            {
                msg.AddTo(new EmailAddress(rec.Email, rec.Name));
            });

            recipients.CarbonCopy.Where(p => !p.Skipped && string.IsNullOrWhiteSpace(p.Error)).ToList().ForEach(rec =>
            {
                msg.AddCc(new EmailAddress(rec.Email, rec.Name));
            });

        }


    }
}
