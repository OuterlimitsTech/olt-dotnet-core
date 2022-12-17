namespace OLT.Core
{
    public record OltAfterCommandQueueItem(IOltCommand Command, IOltCommandResult Result)
    {
        public static OltAfterCommandQueueItem Create(IOltCommand command, IOltCommandResult result)
        {
            return new OltAfterCommandQueueItem(command, result);
        }
    }
}