using System;
using System.Net.Mail;
using System.Text;

namespace OLT.Email
{
    public abstract class OltCalendarInviteArgs<T> : OltBodyArgs<T>
       where T : OltCalendarInviteArgs<T>
    {
        protected internal byte[] CalendarInviteBtyes { get; set; }

        protected OltCalendarInviteArgs()
        {
        }

        /// <summary>
        /// ICS File in Bytes
        /// </summary>
        /// <returns></returns>
        public T WithCalendarInvite(byte[] icsFileBytes)
        {
            if (icsFileBytes == null)
            {
                throw new ArgumentNullException(nameof(icsFileBytes));
            }

            this.CalendarInviteBtyes = icsFileBytes;
            return (T)this;
        }

        /// <summary>
        /// ICS File
        /// </summary>
        /// <returns></returns>
        public T WithCalendarInvite(OltEmailCalendarAttachment attachment)
        {
            if (attachment == null)
            {
                throw new ArgumentNullException(nameof(attachment));
            }

            this.CalendarInviteBtyes = attachment.Bytes;
            return (T)this;
        }

        protected override MailMessage CreateMessage(OltEmailRecipientResult recipients)
        {
            var msg = base.CreateMessage(recipients);

            if (CalendarInviteBtyes != null)
            {
                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType(OltEmailCalendarAttachment.DefaultContentType);
                contentType.Parameters.Add("method", "REQUEST");
                msg.Headers.Add("Content-class", "urn:content-classes:calendarmessage");
                contentType.Parameters.Add("name", OltEmailCalendarAttachment.DefaultFileName);
                AlternateView avCal = AlternateView.CreateAlternateViewFromString(Encoding.UTF8.GetString(CalendarInviteBtyes, 0, CalendarInviteBtyes.Length), contentType);
                msg.AlternateViews.Add(avCal);
            }

            return msg;

            
        }
    }
}
