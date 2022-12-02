using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltCommandHandler : IOltInjectableScoped
    {
        string ActionName { get; }
        Task<IOltCommandValidationResult> ValidateAsync(IOltIdentity identity, IOltCommand command);
        Task<IOltCommandResult> ExecuteAsync(IOltIdentity identity, IOltCommand command);
    }
}