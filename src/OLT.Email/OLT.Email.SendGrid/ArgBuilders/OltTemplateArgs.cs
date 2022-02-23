using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OLT.Email.SendGrid
{
    public abstract class OltCustomArgsArgs<T> : OltUnsubscribeGroupArgs<T>
    where T : OltCustomArgsArgs<T>
    {
        protected internal Dictionary<string, string> CustomArgs { get; set; } = new Dictionary<string, string>();

        protected OltCustomArgsArgs()
        {
        }

        /// <summary>
        /// Adds Custom Arg to Email Request that can be used to assoicate to internal data when subscribing to the Webhooks
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T WithCustomArg(string customArgKey, string customArgValue)
        {
            if (customArgKey == null)
            {
                throw new ArgumentNullException(nameof(customArgKey));
            }
            if (customArgValue == null)
            {
                throw new ArgumentNullException(nameof(customArgValue));
            }
            this.CustomArgs.Add(customArgKey, customArgValue);
            return (T)this;
        }
    }

    public abstract class OltEnableSandboxArgs<T> : OltCustomArgsArgs<T>
        where T : OltEnableSandboxArgs<T>
    {
        protected internal bool SandboxMode { get; set; }

        protected OltEnableSandboxArgs()
        {
        }

        /// <summary>
        /// Enables Sandbox mode in Sendgrid message
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T EnableSandbox()
        {
            this.SandboxMode = true;
            return (T)this;
        }
    }

    public abstract class OltTemplateArgs<T> : OltEnableSandboxArgs<T>
        where T : OltTemplateArgs<T>
    {
        protected internal IOltEmailTemplateId Template { get; set; }

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

            if (CustomArgs.Count > 0)
            {
                msg.AddCustomArgs(CustomArgs);
            }            

            Attachments?.ForEach(attachment => msg.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Bytes), attachment.ContentType));

            ConfigureRecipients(msg, recipients);


            if (SandboxMode)
            {
                msg.MailSettings = new MailSettings
                {
                    SandboxMode = new SandboxMode
                    {
                        Enable = true
                    }
                };
            }

            return msg;
        }

        public override async Task<OltEmailResult> SendAsync()
        {
            if (!IsValid)
            {
                throw new OltSendGridValidationException(ValidationErrors());
            }
            var result = new OltEmailResult();
            var client = CreateClient();
            var msg = CreateMessage(BuildRecipients());
            var json = msg.Serialize();
            var sendResponse = await client.SendEmailAsync(msg);
            if (sendResponse.StatusCode != HttpStatusCode.Accepted)
            {
                var body = await sendResponse.Body.ReadAsStringAsync();
                result.Errors.Add($"{sendResponse.StatusCode} - {body}");
            }
            return result;
        }

    }
}
