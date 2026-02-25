using FluentValidation.Results;

namespace OLT.Core.CommandBus.Tests.Assets.Handlers
{
    public class DuplicateHandlerCommandHandler : OltCommandHandler<DuplicateHandlerCommand>
    {
        protected override Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, DuplicateHandlerCommand command)
        {
            return Task.FromResult<IOltCommandResult>(OltCommandResult.Complete());
        }

        protected override Task<ValidationResult> ValidateAsync(IOltCommandBus commandBus, DuplicateHandlerCommand command)
        {
            return Task.FromResult(new ValidationResult());
        }
    }
}
