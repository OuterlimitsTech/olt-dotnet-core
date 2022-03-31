using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OLT.Constants;
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
        protected string TemplateId { get; set; }   
        protected object TemplateData { get; set; }

        protected OltTemplateArgs()
        {
        }

        /// <summary>
        /// Send Grid TemplateId
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T WithTemplate(string templateId, object templateData)
        {
            if (templateId == null)
            {
                throw new ArgumentNullException(nameof(templateId));
            }

            this.TemplateId = templateId;
            this.TemplateData = templateData;
            return (T)this;
        }

        /// <summary>
        /// Send Grid TemplateId
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T WithTemplate(string templateId)
        {
            return this.WithTemplate(templateId, null);
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
            return this.WithTemplate(template.TemplateId, template.GetTemplateData());
        }

        public override List<string> ValidationErrors()
        {
            var errors = base.ValidationErrors();
            if (string.IsNullOrWhiteSpace(TemplateId))
            {
                errors.Add(OltArgErrorsSendGrid.TemplateId);
            }
            return errors;
        }

        public override SendGridMessage CreateMessage(OltEmailRecipientResult recipients)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(From.Email, From.Name));
            msg.SetTemplateId(TemplateId);

            msg.SetClickTracking(ClickTracking, ClickTracking);

            if (UnsubscribeGroupId.HasValue)
            {
                msg.SetAsm(UnsubscribeGroupId.Value);
            }

            if (TemplateData != null)
            {
                msg.SetTemplateData(TemplateData);
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
            
            var client = CreateClient();
            var msg = CreateMessage(BuildRecipients());
            var sendResponse = await client.SendEmailAsync(msg);

            var result = new OltSendGridEmailResult();
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
