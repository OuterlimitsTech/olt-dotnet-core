﻿using OLT.Core.CommandBus.Tests.Assets;
using System;
using Xunit;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core.CommandBus.Tests.Assets.Handlers;
using OLT.Core.CommandBus.Tests.Assets.EfCore.Entites;
using OLT.Core.CommandBus.Tests.Assets.Models;
using FluentAssertions;

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
                await Assert.ThrowsAsync<OltCommandResultNullException>(() => commandBus.ProcessAsync<UserEntity>(command));
            }

            using (var provider = BuildProvider())
            {
                var commandBus = provider.GetService<IOltCommandBus>();
                var command = new UserEntityCommand();
                await Assert.ThrowsAsync<InvalidCastException>(() => commandBus.ProcessAsync<TestPersonDto>(command));
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

            using (var provider = BuildProvider())
            {
                var commandBus = provider.GetService<IOltCommandBus>();
                var command = new UserEntityCommand();
                var handler = new UserEntityCommandHandler();
                var result = await commandBus.ProcessAsync<UserEntity>(command);
                Assert.Equal(command.ActionName, handler.ActionName);
            }
        }


        [Fact]
        public async Task ValidatorTest()
        {
            using (var provider = BuildProvider())
            {
                var commandBus = provider.GetService<IOltCommandBus>();
                var command = new TestPersonCommand(new TestPersonDto());

                //Command Validation
                var validationResult = await command.ValidateAsync();
                validationResult.Errors.Should().HaveCount(3);
                Assert.False(validationResult.IsValid);

                //Check Command Handler Validation (should be command plus handler errors)
                var result = await commandBus.ValidateAsync(command);
                Assert.False(result.Valid);
                result.Errors.Should().HaveCount(4);


                await Assert.ThrowsAsync<OltValidationException>(() => commandBus.ProcessAsync<TestPersonDto>(command));
            }

            using (var provider = BuildProvider())
            {
                var dto = TestPersonDto.FakerDto();
                var commandBus = provider.GetService<IOltCommandBus>();
                var command = new TestPersonCommand(dto);

                //Command Validation
                var validationResult = await command.ValidateAsync();
                Assert.True(validationResult.IsValid);
                validationResult.Errors.Should().HaveCount(0);

                //Check Command Handler Validation (should be command plus handler errors)
                var result = await commandBus.ValidateAsync(command);
                Assert.True(result.Valid);
                result.Errors.Should().HaveCount(0);

                var actionResult = await commandBus.ProcessAsync<TestPersonDto>(command);
                actionResult.Should().BeEquivalentTo(dto);
            }


        }
    }
}