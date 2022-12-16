using FluentValidation.Results;
using OLT.Core.CommandBus.Tests.Assets.EfCore.Entites;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets.Handlers
{
    public class UserEntityCommandHandler : OltCommandHandler<UserEntityCommand>
    {
        protected override Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, UserEntityCommand command)
        {
            return Task.FromResult<IOltCommandResult>(OltCommandResult.Complete(new UserEntity()));
        }

        protected override Task<ValidationResult> ValidateAsync(IOltCommandBus commandBus, UserEntityCommand command)
        {
            return Task.FromResult(new ValidationResult());
        }
    }
}
