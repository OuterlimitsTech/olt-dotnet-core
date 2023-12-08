using FluentValidation.Results;
using OLT.Core.CommandBus.Tests.Assets.EfCore.Entites;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets
{
    public record DuplicateHandlerCommand : OltCommand
    {
        public override Task<ValidationResult> ValidateAsync()
        {
            return Task.FromResult(new ValidationResult());
        }
    }

    public record DuplicateHandlerWithResultCommand : OltCommand<UserEntity>
    {
        public override Task<ValidationResult> ValidateAsync()
        {
            return Task.FromResult(new ValidationResult());
        }
    }
}
