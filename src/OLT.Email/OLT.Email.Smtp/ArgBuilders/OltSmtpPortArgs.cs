using System;
using System.Collections.Generic;

namespace OLT.Email.Smtp
{
    public abstract class OltSmtpPortArgs<T> : OltSmtpDisableSslArgs<T>
        where T : OltSmtpPortArgs<T>
    {
        protected internal short SmtpPort { get; set; } = 587;

        protected OltSmtpPortArgs()
        {
        }

        /// <summary>
        /// SMTP Server Port
        /// </summary>
        /// <remarks>Default is 587</remarks>
        /// <param name="port"></param>
        /// <returns></returns>
        public T WithSmtpPort(short port)
        {
            if (port <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(port), "must be greater than zero");
            }
            this.SmtpPort = port;
            return (T)this;
        }
  
    }

   
}
