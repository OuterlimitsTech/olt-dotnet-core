namespace OLT.Email.Smtp
{
    public abstract class OltSmtpDisableSslArgs<T> : OltSmtpServerArgs<T>
      where T : OltSmtpDisableSslArgs<T>
    {
        protected internal bool SmtpDisableSSL { get; set; }

        protected OltSmtpDisableSslArgs()
        {
        }

        /// <summary>
        /// Disabled SSL for SMTP
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public T WithSmtpDisabledSSL(bool value)
        {
            this.SmtpDisableSSL = value;
            return (T)this;
        }

    }

   
}
