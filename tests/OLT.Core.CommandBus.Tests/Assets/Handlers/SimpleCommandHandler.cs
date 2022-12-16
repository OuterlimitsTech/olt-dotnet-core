using FluentValidation.Results;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets.Handlers
{
    public class SimpleCommandHandler : OltCommandHandler<SimpleCommand>
    {
        protected override Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, SimpleCommand command)
        {
            return Task.FromResult<IOltCommandResult>(OltCommandResult.Complete());
        }

        protected override Task<ValidationResult> ValidateAsync(IOltCommandBus commandBus, SimpleCommand command)
        {
            return Task.FromResult(new ValidationResult());
        }
    }
}
