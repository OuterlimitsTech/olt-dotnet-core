namespace OLT.Email
{
    public class OltSmtpServer 
    {
        public virtual string Host { get; set; }
        public virtual int? Port { get; set; }
        public virtual bool DisableSsl { get; set; }
        public virtual OltSmtpCredentials Credentials { get; set; } = new OltSmtpCredentials();
    }
}