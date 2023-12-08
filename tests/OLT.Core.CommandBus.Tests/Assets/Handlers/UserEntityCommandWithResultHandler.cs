using FluentValidation.Results;
using OLT.Core.CommandBus.Tests.Assets.EfCore.Entites;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets.Handlers
{
    public class UserEntityCommandWithResultHandler : OltCommandHandler<UserEntityCommandWithResult, UserEntity>
    {
        protected override Task<UserEntity> ExecuteAsync(IOltCommandBus commandBus, UserEntityCommandWithResult command)
        {
            return Task.FromResult(new UserEntity());
        }

        protected override Task<ValidationResult> ValidateAsync(IOltCommandBus commandBus, UserEntityCommandWithResult command)
        {
            return Task.FromResult(new ValidationResult());
        }
    }
}


