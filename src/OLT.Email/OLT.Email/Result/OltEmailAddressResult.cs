﻿using System;

namespace OLT.Email
{
    public class OltEmailAddressResult : OltEmailAddress
    {
        /// <summary>
        /// Constructs Email Result 
        /// </summary>
        /// <param name="copyFrom"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public OltEmailAddressResult(IOltEmailAddress copyFrom, OltEmailBuilderArgs args)
        {
            if (copyFrom == null)
            {
                throw new ArgumentNullException(nameof(copyFrom));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Name = copyFrom.Name;
            Email = copyFrom.Email;

            if (!args.AllowSend(Email))
            {
                SkipReason = OltErrorMessages.WhitelistSkip;
            }
        }

        public virtual bool Success => Sent;
        public virtual bool Sent => !Skipped && string.IsNullOrWhiteSpace(Error);
        public virtual bool Skipped => !string.IsNullOrWhiteSpace(SkipReason);

        public virtual string SkipReason { get; set; }
        public virtual string Error { get; set; }
        
    }
}