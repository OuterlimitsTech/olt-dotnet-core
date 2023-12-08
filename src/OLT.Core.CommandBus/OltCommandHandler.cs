using FluentValidation.Results;
using System;
using System.Threading.Tasks;

namespace OLT.Core
{
    public abstract class OltCommandHandler<TCommand> : OltDisposable, IOltCommandHandler
        where TCommand : notnull, IOltCommand
    {
        public virtual string ActionName => typeof(TCommand).FullName;

        protected abstract Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, TCommand command);

        public virtual Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, IOltCommand command)
        {
            return ExecuteAsync(commandBus, (TCommand)command);
        }

        protected abstract Task<ValidationResult> ValidateAsync(IOltCommandBus commandBus, TCommand command);

        public virtual async Task<IOltCommandValidationResult> ValidateAsync(IOltCommandBus commandBus, IOltCommand command)
        {
            var result = await ValidateAsync(commandBus, (TCommand)command);
            var commandValid = await command.ValidateAsync();
            return OltCommandValidationResult.FromResult(result, commandValid);
        }
    }


    public abstract class OltCommandHandler<TCommand, TResult> : OltDisposable, IOltCommandHandler<TResult>, IOltPostCommandHandler<TResult>
          where TCommand : notnull, IOltCommand<TResult>
          where TResult : notnull
    {
        public virtual string ActionName => typeof(TCommand).FullName;

        protected abstract Task<TResult> ExecuteAsync(IOltCommandBus commandBus, TCommand command);

        public virtual async Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, IOltCommand command)
        {
            var result = await ExecuteAsync(commandBus, (IOltCommand<TResult>)command);
            return new OltCommandResult(result);
        }

        public virtual Task<TResult> ExecuteAsync(IOltCommandBus commandBus, IOltCommand<TResult> command)
        {
            return ExecuteAsync(commandBus, (TCommand)command);
        }

        protected abstract Task<ValidationResult> ValidateAsync(IOltCommandBus commandBus, TCommand command);

        public virtual async Task<IOltCommandValidationResult> ValidateAsync(IOltCommandBus commandBus, IOltCommand command)
        {
            var result = await ValidateAsync(commandBus, (TCommand)command);
            var commandValid = await command.ValidateAsync();
            return OltCommandValidationResult.FromResult(result, commandValid);
        }

        
        public virtual Task PostExecuteAsync(IOltCommand command, TResult result)
        {
            return PostExecuteAsync((TCommand)command, result);
        }

        public virtual Task PostExecuteAsync(TCommand command, TResult result)
        {
            return Task.CompletedTask;
        }
    }

}
