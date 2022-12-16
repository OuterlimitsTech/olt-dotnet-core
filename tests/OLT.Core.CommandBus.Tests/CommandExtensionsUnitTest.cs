using System;
using Xunit;
using OLT.Core.CommandBus.Tests.Assets.EfCore.Entites;
using FluentAssertions;

namespace OLT.Core.CommandBus.Tests
{
    public class CommandExtensionsUnitTest
    {
        
        [Fact]
        public void ToResultTests()
        {
            var entity = UserEntity.FakerEntity();
            var commandResult = OltCommandResult.Complete(entity);
            var result = OltCommandExtensions.ToResult<UserEntity>(commandResult);
            result.Should().BeEquivalentTo(entity);
        }

        [Fact]
        public void ExceptionTests()
        {
            Assert.Throws<OltCommandResultNullException>(() => OltCommandExtensions.ToResult<UserEntity>(OltCommandResult.Complete()));
            Assert.Throws<InvalidCastException>(() => OltCommandExtensions.ToResult<UserEntity>(OltCommandResult.Complete(new AddresssEntity())));
        }
    }
}