namespace OLT.Email
{
    public class OltApplicationErrorEmail : OltSmtpEmail, IOltApplicationErrorEmail
    {
        public string AppName { get; set; }
        public string Environment { get; set; }
    }
}