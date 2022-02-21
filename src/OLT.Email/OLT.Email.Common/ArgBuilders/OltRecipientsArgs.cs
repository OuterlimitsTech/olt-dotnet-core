﻿using System.Collections.Generic;
using System.Linq;

namespace OLT.Email
{
    public abstract class OltRecipientsArgs<T> : OltAttachmentsArgs<T>
      where T : OltRecipientsArgs<T>
    {
        protected internal List<OltEmailAddress> To { get; set; } = new List<OltEmailAddress>();
        protected internal List<OltEmailAddress> CarbonCopy { get; set; } = new List<OltEmailAddress>();

        protected OltRecipientsArgs()
        {
        }

        /// <summary>
        /// Recipients to send to
        /// </summary>
        /// <returns></returns>
        public T WithRecipients(OltEmailRecipients value)
        {
            value.To?.ToList().ForEach(rec =>
            {
                To.Add(new OltEmailAddress(rec.Email, rec.Name));
            });

            value.CarbonCopy?.ToList().ForEach(rec =>
            {
                CarbonCopy.Add(new OltEmailAddress(rec.Email, rec.Name));
            });

            return (T)this;
        }
        
        protected OltEmailRecipientResult BuildRecipients()
        {
            var recipientResult = new OltEmailRecipientResult();

            To.ForEach(rec =>
            {
                if (!recipientResult.To.Any(x => x.Email.Equals(rec.Email, System.StringComparison.OrdinalIgnoreCase)))
                {
                    recipientResult.To.Add(new OltEmailAddressResult(rec, this));
                }
            });

            CarbonCopy.ForEach(rec =>
            {
                if (!recipientResult.To.Any(x => x.Email.Equals(rec.Email, System.StringComparison.OrdinalIgnoreCase)) &&
                    !recipientResult.CarbonCopy.Any(x => x.Email.Equals(rec.Email, System.StringComparison.OrdinalIgnoreCase)))
                {
                    recipientResult.CarbonCopy.Add(new OltEmailAddressResult(rec, this));
                }
            });

            return recipientResult;
        }

        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (!To.Any())
            {
                errors.Add("Requires To Recipient");
            }
            return errors;
        }
    }

}