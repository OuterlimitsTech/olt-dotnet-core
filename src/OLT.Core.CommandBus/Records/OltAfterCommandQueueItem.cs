namespace OLT.Core
{
    public record OltAfterCommandQueueItem(IOltPostCommandHandler Handler, IOltCommand Command, IOltCommandResult Result)
    {
        public static OltAfterCommandQueueItem Create(IOltPostCommandHandler handler, IOltCommand command, IOltCommandResult result)
        {
            return new OltAfterCommandQueueItem(handler, command, result);
        }
    }
}