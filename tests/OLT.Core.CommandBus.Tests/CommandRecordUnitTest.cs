using OLT.Core.CommandBus.Tests.Assets;
using Xunit;

namespace OLT.Core.CommandBus.Tests
{

    public class CommandRecordUnitTest
    {
        [Fact]
        public void CorrelationIdTest()
        {
            var command = new UserEntityCommand();
            var anotherCommand = new UserEntityCommand();
            var anotherCommandId = anotherCommand.CorrelationId;
            var commandId = command.CorrelationId;

            for(var idx = 0; idx < 10; idx++)
            {
                Assert.Equal(commandId, command.CorrelationId); //Make sure it doesn't change
                Assert.Equal(anotherCommandId, anotherCommand.CorrelationId); //Make sure it doesn't change
            }

            Assert.NotEqual(commandId, anotherCommandId);
            Assert.NotEqual(commandId, anotherCommand.CorrelationId);
            Assert.NotEqual(command.CorrelationId, anotherCommand.CorrelationId);
        }


        [Fact]
        public void ActionNameTest()
        {
            var testCommand = new UserEntityCommand();
            var simpleCommand = new SimpleCommand();

            Assert.Equal(testCommand.ActionName, new UserEntityCommand().ActionName);
            Assert.Equal(simpleCommand.ActionName, new SimpleCommand().ActionName);
            Assert.NotEqual(simpleCommand.ActionName, testCommand.ActionName);
        }

    }

}