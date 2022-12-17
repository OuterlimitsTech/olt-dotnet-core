using FluentValidation.Results;
using OLT.Core.CommandBus.Tests.Assets.Models;
using OLT.Core.CommandBus.Tests.Assets.Validators;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets
{
    public record TestPersonCommand(TestPersonDto Model) : OltCommand
    {
        public static TestPersonCommand Create(TestPersonDto model)
        {
            return new TestPersonCommand(model);
        }

        public override async Task<ValidationResult> ValidateAsync()
        {
            var validator = new TestPersonDtoValidator();
            return await validator.ValidateAsync(Model);
        }
    }

}
