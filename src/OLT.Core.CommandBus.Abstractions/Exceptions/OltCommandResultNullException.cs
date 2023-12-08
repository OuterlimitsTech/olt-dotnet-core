namespace OLT.Core
{
    public class OltCommandResultNullException : OltException
    {
        public OltCommandResultNullException() : base($"{nameof(IOltCommandResult)}.{nameof(IOltCommandResult.GetResult)} is null")
        {
        }
    }
}