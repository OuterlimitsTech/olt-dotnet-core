namespace OLT.Core
{
    public class OltCommandHandlerNotFoundException : OltException
    {
        public OltCommandHandlerNotFoundException(IOltCommand command) : base($"Unable to locate command handler {command.ActionName} for command {command.GetType().FullName}")
        {
        }


    }
}