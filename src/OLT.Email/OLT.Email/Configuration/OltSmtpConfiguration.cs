namespace OLT.Email
{
    public class OltSmtpConfiguration : OltEmailConfiguration
    {
        public OltSmtpServer Smtp {  get; set; } = new OltSmtpServer();
    }
}