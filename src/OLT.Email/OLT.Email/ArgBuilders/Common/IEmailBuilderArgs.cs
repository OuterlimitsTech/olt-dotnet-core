namespace OLT.Email
{
    public interface IEmailBuilderArgs
    {
        bool AllowSend(string emailAddress);
    }
}
