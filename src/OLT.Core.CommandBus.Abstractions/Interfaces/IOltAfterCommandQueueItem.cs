using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltAfterCommandQueueItem
    {   
        Task PostExecuteAsync(IOltCommandBus commandBus);
    }

}