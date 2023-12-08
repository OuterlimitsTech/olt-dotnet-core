using FluentValidation.Results;
using OLT.Core.CommandBus.Tests.Assets.EfCore.Entites;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets
{
    public record TypedWithUnTypeHandlerCommand : OltCommand<UserEntity>
    {
        public override Task<ValidationResult> ValidateAsync()
        {
            return Task.FromResult(new ValidationResult());
        }
    }
}
