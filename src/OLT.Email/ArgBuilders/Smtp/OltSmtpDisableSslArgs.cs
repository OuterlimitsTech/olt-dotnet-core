namespace OLT.Email
{
    public abstract class OltSmtpDisableSslArgs<T> : OltSmtpHostArgs<T>
      where T : OltSmtpDisableSslArgs<T>
    {
        protected  bool SmtpSSLDisabled { get; set; }

        protected OltSmtpDisableSslArgs()
        {
        }

        /// <summary>
        /// Disabled SSL for SMTP
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public T WithSmtpSSLDisabled(bool value)
        {
            this.SmtpSSLDisabled = value;
            return (T)this;
        }

    }

   
}
