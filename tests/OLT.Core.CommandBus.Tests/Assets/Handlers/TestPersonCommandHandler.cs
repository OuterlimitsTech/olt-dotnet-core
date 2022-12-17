using FluentValidation.Results;
using OLT.Core.CommandBus.Tests.Assets.Models;
using OLT.Core.CommandBus.Tests.Assets.Validators;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets.Handlers
{
    public class TestPersonCommandHandler : OltCommandHandler<TestPersonCommand>
    {
        protected override Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, TestPersonCommand command)
        {
            return Task.FromResult<IOltCommandResult>(OltCommandResult.Complete(command.Model));
        }

        protected override async Task<ValidationResult> ValidateAsync(IOltCommandBus commandBus, TestPersonCommand command)
        {
            var validator = new TestPersonCommandValidator();
            var dto = new TestCommandDtoHelper
            {
                Value = command.Model.Id
            };
            return await validator.ValidateAsync(dto);
        }

        public override Task PostExecuteAsync(TestPersonCommand command, IOltCommandResult result)
        {            
            return Task.CompletedTask;
        }
    }
}
