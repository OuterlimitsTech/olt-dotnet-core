using OLT.Core;

namespace OLT.Email.Smtp
{
    public class OltSmtpServer : IOltSmtpServer
    {
        public OltSmtpServer()
        {

        }

        public OltSmtpServer(IOltSmtpServer server)
        {
            Host = server.Host;
            Port = server.Port;
            DisableSsl = server.DisableSsl;
        }

        public virtual string Host { get; set; }
        public virtual short? Port { get; set; }
        public virtual bool DisableSsl { get; set; }
    }

    public class OltSmtpConfiguration : OltEmailConfiguration, IOltSmtpConfiguration
    {
        public OltSmtpConfiguration()
        {

        }

        public OltSmtpConfiguration(IOltSmtpServer server)
        {
            Smtp = new OltSmtpServer(server);
        }

        public IOltSmtpServer Smtp {  get; set; } = new OltSmtpServer();
    }
}