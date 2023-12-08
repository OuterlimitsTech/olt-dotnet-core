namespace OLT.Core
{
    public class OltCommandHandlerMultipleException : OltException
    {
        public OltCommandHandlerMultipleException(IOltCommand command) : base($"Muliple command handlers found for {command.ActionName} for command {command.GetType().FullName}")
        {
        }
    }
}