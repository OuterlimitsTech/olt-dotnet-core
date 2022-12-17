namespace OLT.Core
{
    public class OltCommandResultNullException : OltException
    {
        public OltCommandResultNullException() : base($"{nameof(OltCommandResult)}.{nameof(OltCommandResult.Result)} is null")
        {
        }
    }
}