using FluentValidation.Results;
using System.Threading.Tasks;

namespace OLT.Core
{
    public abstract class OltCommandHandler<TCommand> : OltDisposable, IOltCommandHandler
        where TCommand : notnull, IOltCommand
    {
        public virtual string ActionName => typeof(TCommand).FullName;

        public virtual async Task<IOltCommandValidationResult> ValidateAsync(IOltCommandBus commandBus, IOltCommand command)
        {
            var result = await ValidateAsync(commandBus, (TCommand)command);
            var commandValid = await command.ValidateAsync();
            return OltCommandValidationResult.FromResult(result, commandValid);
        }

        public virtual async Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, IOltCommand command)
        {
            return await ExecuteAsync(commandBus, (TCommand)command);
        }

        protected abstract Task<ValidationResult> ValidateAsync(IOltCommandBus commandBus, TCommand command);
        protected abstract Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, TCommand command);

        public virtual Task PostExecuteAsync(IOltCommandResult result)
        {
            return Task.CompletedTask;
        }
    }
  
}