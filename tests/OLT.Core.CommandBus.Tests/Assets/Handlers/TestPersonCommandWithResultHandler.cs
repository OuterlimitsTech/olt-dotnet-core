using AwesomeAssertions;
using FluentValidation.Results;
using OLT.Core.CommandBus.Tests.Assets.Models;
using OLT.Core.CommandBus.Tests.Assets.Validators;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets.Handlers
{
    public class TestPersonCommandWithResultHandler : OltCommandHandler<TestPersonCommandWithResult, TestPersonDto>
    {
        private TestPersonDto _dto;
        private TestPersonCommandWithResult _command;

        protected override Task<TestPersonDto> ExecuteAsync(IOltCommandBus commandBus, TestPersonCommandWithResult command)
        {
            _command = command;
            _dto = TestPersonDto.FakerDto();
            return Task.FromResult(_dto);
        }

        protected override async Task<ValidationResult> ValidateAsync(IOltCommandBus commandBus, TestPersonCommandWithResult command)
        {

            var validator = new TestPersonCommandValidator();
            var dto = new TestCommandDtoHelper
            {
                Value = command.Model.Id
            };
            return await validator.ValidateAsync(dto);
        }

        public override Task PostExecuteAsync(IOltCommand command, TestPersonDto result)
        {
            result.Should().Be(_dto);
            command.Should().Be(_command);
            return Task.CompletedTask;
        }
    }
}
