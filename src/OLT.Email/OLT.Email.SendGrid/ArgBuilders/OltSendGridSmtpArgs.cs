namespace OLT.Email.SendGrid
{
    public abstract class OltSendGridSmtpArgs<T> : OltApiKeyArgs<T>
        where T : OltSendGridSmtpArgs<T>
    {
        protected OltSendGridSmtpArgs()
        {
        }
    }
}
