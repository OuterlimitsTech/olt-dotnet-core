namespace OLT.Core
{
    public record OltAfterCommandQueueItem(IOltPostCommandHandler Handler, IOltCommand Command, IOltCommandResult Result)
    {

    }
}