using FluentAssertions;
using FluentValidation.Results;
using OLT.Core.CommandBus.Tests.Assets.Models;
using OLT.Core.CommandBus.Tests.Assets.Validators;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets.Handlers
{
    public class TestPersonCommandHandler : OltCommandHandler<TestPersonCommand>, IOltPostCommandHandler<TestPersonCommand>
    {
        private TestPersonDto _dto;
        private TestPersonCommand _command;

        protected override Task<IOltCommandResult> ExecuteAsync(IOltCommandBus commandBus, TestPersonCommand command)
        {
            _command = command;
            _dto = TestPersonDto.FakerDto();
            return Task.FromResult<IOltCommandResult>(OltCommandResult.Complete(_dto));
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

        public Task PostExecuteAsync(TestPersonCommand command, IOltCommandResult result)
        {
            var resultDto = result.GetResult<TestPersonDto>();
            resultDto.Should().Be(_dto);
            command.Should().Be(_command);
            return Task.CompletedTask;
        }
    }
}
