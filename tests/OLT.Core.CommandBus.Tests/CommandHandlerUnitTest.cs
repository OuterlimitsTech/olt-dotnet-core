using OLT.Core.CommandBus.Tests.Assets;
using System.Net.Http;
using System;
using Xunit;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core.CommandBus.Tests.Assets.Handlers;
using OLT.Core.CommandBus.Tests.Assets.EfCore.Entites;

namespace OLT.Core.CommandBus.Tests
{

    public class CommandHandlerUnitTest : BaseUnitTests
    {

        [Fact]
        public async Task ExceptionTests()
        {
            using (var provider = BuildProvider())
            {
                var commandBus = provider.GetService<IOltCommandBus>();
                var command = new DuplicateHandlerCommand();
                await Assert.ThrowsAsync<OltCommandHandlerMultipleException>(() => commandBus.ProcessAsync(command));
            }

            using (var provider = BuildProvider())
            {
                var commandBus = provider.GetService<IOltCommandBus>();
                var command = new NoHandlerCommand();
                await Assert.ThrowsAsync<OltCommandHandlerNotFoundException>(() => commandBus.ProcessAsync(command));
            }

            using (var provider = BuildProvider())
            {
                var commandBus = provider.GetService<IOltCommandBus>();
                var command = new SimpleCommand();
                await Assert.ThrowsAsync<NullReferenceException>(() => commandBus.ProcessAsync<UserEntity>(command));
            }

            using (var provider = BuildProvider())
            {
                var commandBus = provider.GetService<IOltCommandBus>();
                var command = new UserEntityCommand();
                await Assert.ThrowsAsync<InvalidCastException>(() => commandBus.ProcessAsync<AddresssEntity>(command));
            }            
        }


        [Fact]
        public async Task ResultCastTest()
        {
            using (var provider = BuildProvider())
            {
                var commandBus = provider.GetService<IOltCommandBus>();
                var command = new UserEntityCommand();
                var result = await commandBus.ProcessAsync<UserEntity>(command);
                Assert.Equal(typeof(UserEntity), result.GetType());
            }

        }
    }
}