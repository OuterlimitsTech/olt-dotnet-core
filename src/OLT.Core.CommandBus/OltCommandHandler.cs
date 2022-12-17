using FluentValidation.Results;
using System.Threading.Tasks;

namespace OLT.Core
{
    public abstract class OltCommandHandler<TCommand> : OltDisposable, IOltCommandHandler
        where TCommand : notnull, IOltCommand
    {
        public virtual string ActionName => typeof(TCommand).FullName;


        protected abstract Task<ValidationResult> ValidateAsync(IOltCommandBus commandBus, TCommand command);

        public virtual async Task<IOltCommandValidationResult> ValidateAsync(IOltCommandBus commandBus, IOltCommand command)
        {
            var result = await ValidateAsync(commandBus, (TCommand)command);
            var commandValid = await command.ValidateAsync();
            return OltCommandValidationResult.FromResult(result, commandValid);
        }        

        protected abstract Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, TCommand command);

        public virtual Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, IOltCommand command)
        {
            return ExecuteAsync(commandBus, (TCommand)command);
        }
    }
  
}