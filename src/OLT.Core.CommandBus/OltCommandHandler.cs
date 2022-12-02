using FluentValidation.Results;
using System.Threading.Tasks;

namespace OLT.Core
{
    public abstract class OltCommandHandler<TCommand> : OltDisposable, IOltCommandHandler
        where TCommand : notnull, IOltCommand
    {
        public virtual string ActionName => typeof(TCommand).FullName;

        public virtual async Task<IOltCommandValidationResult> ValidateAsync(IOltIdentity identity, IOltCommand command)
        {
            var result = await ValidateAsync(identity, (TCommand)command);
            var commandValid = await command.ValidateAsync();
            return OltCommandValidationResult.FromResult(result, commandValid);
        }

        public virtual async Task<IOltCommandResult> ExecuteAsync(IOltIdentity identity, IOltCommand command)
        {
            return await ExecuteAsync(identity, (TCommand)command);
        }

        protected abstract Task<ValidationResult> ValidateAsync(IOltIdentity identity, TCommand command);
        protected abstract Task<IOltCommandResult> ExecuteAsync(IOltIdentity identity, TCommand command);

    }
}