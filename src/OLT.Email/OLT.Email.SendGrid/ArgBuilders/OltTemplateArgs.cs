using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OLT.Email.SendGrid
{

    public abstract class OltTemplateArgs<T> : OltEnableSandboxArgs<T>
        where T : OltTemplateArgs<T>
    {
        protected IOltEmailTemplateId Template { get; set; }

        protected OltTemplateArgs()
        {
        }

        /// <summary>
        /// Send Grid Template
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T WithTemplate(IOltEmailTemplateId template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }
            this.Template = template;            
            return (T)this;
        }

        public override List<string> ValidationErrors()
        {
            var errors = base.ValidationErrors();
            if (string.IsNullOrWhiteSpace(Template?.TemplateId))
            {
                errors.Add(OltArgErrorsSendGrid.TemplateId);
            }
            return errors;
        }

        public override SendGridMessage CreateMessage(OltEmailRecipientResult recipients)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(From.Email, From.Name));
            msg.SetTemplateId(Template.TemplateId);

            msg.SetClickTracking(ClickTracking, ClickTracking);

            if (UnsubscribeGroupId.HasValue)
            {
                msg.SetAsm(UnsubscribeGroupId.Value);
            }

            var templateData = Template.GetTemplateData();
            if (templateData != null)
            {
                msg.SetTemplateData(templateData);
            }

            if (CustomArgs.Count > 0)
            {
                msg.AddCustomArgs(CustomArgs);
            }            

            Attachments.ForEach(attachment => msg.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Bytes), attachment.ContentType));

            ConfigureRecipients(msg, recipients);

            msg.SetSandBoxMode(SandboxMode);

            return msg;
        }

        public override async Task<OltSendGridEmailResult> SendAsync()
        {
            if (!IsValid)
            {
                throw new OltSendGridValidationException(ValidationErrors());
            }

            var result = new OltSendGridEmailResult();
            var client = CreateClient();
            var msg = CreateMessage(BuildRecipients());
            var sendResponse = await client.SendEmailAsync(msg);

            if (sendResponse.StatusCode != HttpStatusCode.Accepted)
            {
                var body = await sendResponse.Body.ReadAsStringAsync();

                result.Errors.Add($"{sendResponse.StatusCode}");
                result.SendGrid = JsonConvert.DeserializeObject<OltSendGridResponseJson>(body);
                result.SendGrid.Errors.ForEach(error =>
                {
                    result.Errors.Add($"{error.Field} - {error.Message}");
                });

                
            }
            return result;
        }

    }
}
