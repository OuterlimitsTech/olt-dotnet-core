using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OLT.Core
{
    public interface IOltAfterCommandQueueItem
    {   
        Task PostExecuteAsync(IOltCommandBus commandBus);
    }

    public record OltAfterCommandQueueItem(IOltPostCommandHandler Handler, IOltCommand Command, IOltCommandResult Result) : IOltAfterCommandQueueItem
    {
        public Task PostExecuteAsync(IOltCommandBus commandBus)
        {
            return Handler.PostExecuteAsync(Command, Result);
        }
    }

    public record OltAfterCommandQueueItem<TResult>(IOltPostCommandHandler<TResult> Handler, IOltCommand Command, TResult Result) : IOltAfterCommandQueueItem
      where TResult : notnull
    {
        public Task PostExecuteAsync(IOltCommandBus commandBus)
        {
            return Handler.PostExecuteAsync(Command, Result);
        }
    }

}