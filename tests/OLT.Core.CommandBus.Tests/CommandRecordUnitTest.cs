using OLT.Core.CommandBus.Tests.Assets;
using OLT.Core.CommandBus.Tests.Assets.Models;
using Xunit;

namespace OLT.Core.CommandBus.Tests
{

    public class CommandRecordUnitTest
    {

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