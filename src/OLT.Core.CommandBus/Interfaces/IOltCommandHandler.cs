using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltCommandHandler : IOltInjectableScoped
    {
        string ActionName { get; }

        /// <summary>
        /// Can execute the command? <paramref name="commandBus"/> allows for nested command validation
        /// </summary>
        /// <param name="commandBus"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<IOltCommandValidationResult> ValidateAsync(IOltCommandBus commandBus, IOltCommand command);


        /// <summary>
        /// Run the Command. <paramref name="commandBus"/> allows for nested command execution
        /// </summary>
        /// <param name="commandBus"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, IOltCommand command);
                
    }
}