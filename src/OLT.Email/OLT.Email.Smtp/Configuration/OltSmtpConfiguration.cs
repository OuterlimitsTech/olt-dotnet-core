namespace OLT.Email.Smtp
{
    public class OltSmtpConfiguration : OltEmailConfiguration
    {
        public OltSmtpServer Smtp {  get; set; } = new OltSmtpServer();
    }
}