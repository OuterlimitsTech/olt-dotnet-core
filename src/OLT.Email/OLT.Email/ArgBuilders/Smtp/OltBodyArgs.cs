using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace OLT.Email
{
    public abstract class OltBodyArgs<T> : OltSubjectLineArgs<T>
        where T : OltBodyArgs<T>
    {
        protected internal string Body { get; set; }

        protected OltBodyArgs()
        {
        }

        /// <summary>
        /// Email Body
        /// </summary>
        /// <returns></returns>
        public T WithBody(string body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            this.Body = body;
            return (T)this;
        }

        /// <summary>
        /// Sets email body and subject to format an error
        /// </summary>
        /// <returns></returns>
        public T WithAppError(Exception exception, string appName, string environment)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            this.SubjectLine = $"[{appName}] APPLICATION ERROR in {environment} Environment occurred at {DateTimeOffset.Now:f}";
            this.Body = $"The following error occurred:{Environment.NewLine}{exception}";
            return (T)this;
        }

        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(Body))
            {
                errors.Add(OltSmtpArgErrors.Body);
            }
            return errors;
        }

        public virtual MailMessage CreateMessage(OltEmailRecipientResult recipients)
        {
            var msg = new MailMessage
            {
                From = new MailAddress(From.Email, From.Name),
                Subject = SubjectLine,
                Body = Body,
            };

            ConfigureRecipients(msg, recipients);

            return msg;
        }
    }
}
