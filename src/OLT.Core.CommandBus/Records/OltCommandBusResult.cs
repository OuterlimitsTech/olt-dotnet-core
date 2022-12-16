namespace OLT.Core
{
    public record OltCommandBusResult(string CorrelationId, string ActionName, IOltCommandResult CommandResult) : IOltCommandBusResult
    {
        public static OltCommandBusResult FromCommand(IOltCommand command, IOltCommandResult commandResult)
        {
            return new OltCommandBusResult(command.CorrelationId, command.ActionName, commandResult);
        }
    }
}