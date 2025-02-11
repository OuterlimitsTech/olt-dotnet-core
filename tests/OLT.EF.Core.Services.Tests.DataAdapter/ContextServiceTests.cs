﻿using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.Core.Common.Tests.Assets;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Models;
using OLT.EF.Core.Services.Tests.Lib;
using OLT.EF.Core.Services.Tests.Lib.Abstract;

namespace OLT.EF.Core.Services.Tests.DataAdapter;

public class ContextServiceTests : BaseUnitTests
{
 
    [Fact]
    public void QueryableSearchers()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();
            var model = service.CreatePerson();
            var result = service.Get(false, new PersonNameFirstStartsWithSearcher(model.NameFirst), new PersonNameLastStartsWithSearcher(model.NameLast));
            Assert.Equal(model.Id, result?.Id);
        }
    }


    [Fact]
    public void QueryableSearcher()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();
            var model = service.CreatePerson();
            var result = service.Get(new OltSearcherGetById<PersonEntity>(model.Id));
            Assert.Equal(model.Id, result?.Id);
        }
    }

    [Fact]
    public void Get()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();

            var personEntity = service.CreatePerson();
            var userEntity = service.CreateUser();
            Assert.True(service.GetAllPeople().Any());
            Assert.True(service.GetAllPeopleSearcher().Any());
            Assert.True(service.GetAllPeopleOrdered().Any());
            Assert.True(service.GetAllUsers().Any());
            Assert.True(service.GetAllUsersSearcher().Any());
            Assert.True(service.GetAllDtoUsers().Any());
            Assert.True(service.GetAllDtoUsersSearcher().Any());
            Assert.True(service.GetPeopleOrdered(false).Any());
            Assert.True(service.GetPeopleOrdered(true).Any());
            Assert.True(service.GetPeopleOrdered(new OltSearcherGetAll<PersonEntity>(false)).Any());
            Assert.True(service.GetPeopleOrdered(new OltSearcherGetAll<PersonEntity>(true)).Any());
            Assert.True(service.GetPeopleOrdered(new OltSearcherGetAll<PersonEntity>(true), new OltSearcherGetById<PersonEntity>(personEntity.Id)).Any());
            Assert.NotNull(service.GetDtoUser(new OltSearcherGetById<UserEntity>(userEntity.Id)));
            Assert.NotNull(service.GetDtoUser(userEntity.Id));
        }
    }

    [Fact]
    public async Task GetAsync()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();
            var adapters = provider.GetServices<IOltAdapter>().ToList();

            var personEntity = service.CreatePerson();
            var userEntity = service.CreateUser();

            Assert.True((await service.GetAllPeopleAsync()).Any());
            Assert.True((await service.GetAllPeopleSearcherAsync()).Any());
            Assert.True((await service.GetAllUsersAsync()).Any());
            Assert.True((await service.GetAllUsersOrderedAsync()).Any());
            Assert.True((await service.GetAllUsersSearcherAsync()).Any());
            Assert.True((await service.GetAllDtoUsersAsync()).Any());
            Assert.True((await service.GetAllDtoUsersSearcherAsync()).Any());
            Assert.NotNull(await service.GetDtoUserAsync(new OltSearcherGetById<UserEntity>(userEntity.Id)));
            Assert.NotNull(await service.GetDtoUserAsync(userEntity.Id));
        }
    }

    [Fact]
    public void DeletedTest()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();

            var personEntity = service.CreatePerson();
            Assert.NotNull(service.Get(false).FirstOrDefault(p => p.Id == personEntity.Id));
            Assert.NotNull(service.Get(true).FirstOrDefault(p => p.Id == personEntity.Id));
            Assert.NotNull(service.GetNonDeleted().FirstOrDefault(p => p.Id == personEntity.Id));
            Assert.True(service.Delete<PersonEntity>(personEntity.Id));
            Assert.Null(service.Get(false).FirstOrDefault(p => p.Id == personEntity.Id));
            Assert.NotNull(service.Get(true).FirstOrDefault(p => p.Id == personEntity.Id));
            Assert.Null(service.GetNonDeleted().FirstOrDefault(p => p.Id == personEntity.Id));

            var userEntity = service.CreateUser();
            Assert.Equal(userEntity.Id, service.GetDtoUser(userEntity.Id)?.UserId);
            Assert.Throws<InvalidCastException>(() => service.Delete<UserEntity>(userEntity.Id));
        }


    }

    [Fact]
    public async Task DeletedTestAsync()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();

            var personEntity = service.CreatePerson();
            Assert.NotNull(service.Get(false).FirstOrDefault(p => p.Id == personEntity.Id));
            Assert.NotNull(service.Get(true).FirstOrDefault(p => p.Id == personEntity.Id));
            Assert.NotNull(service.GetNonDeleted().FirstOrDefault(p => p.Id == personEntity.Id));
            Assert.True(await service.DeleteAsync<PersonEntity>(personEntity.Id));
            Assert.Null(service.Get(false).FirstOrDefault(p => p.Id == personEntity.Id));
            Assert.NotNull(service.Get(true).FirstOrDefault(p => p.Id == personEntity.Id));
            Assert.Null(service.GetNonDeleted().FirstOrDefault(p => p.Id == personEntity.Id));

            var userEntity = service.CreateUser();
            Assert.Equal(userEntity.Id, (await service.GetDtoUserAsync(new OltSearcherGetById<UserEntity>(userEntity.Id)))?.UserId);
            Assert.Equal(userEntity.Id, (await service.GetDtoUserAsync(userEntity.Id))?.UserId);
            await Assert.ThrowsAsync<InvalidCastException>(() => service.DeleteAsync<UserEntity>(userEntity.Id));
        }


    }



    [Fact]
    public async Task DbTransactionTestsAsync()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();
            await service.DbTransactionAsync(false);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();
            try
            {
                await service.DbTransactionAsync(true);
                Assert.True(false);
            }
            catch
            {
                Assert.True(true);
            }
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();
            var dto = await service.DbTransactionAsync<UserDto>(false);
            Assert.NotNull(dto);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();
            try
            {
                var dto = await service.DbTransactionAsync<UserDto>(true);
                Assert.True(false);
            }
            catch
            {
                Assert.True(true);
            }
        }
    }

    [Fact]
    public async Task DbTransactionNestedTestsAsync()
    {

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();
            var context = provider.GetRequiredService<TestDbContext>();
            using var transaction = await context.Database.BeginTransactionAsync();
            await service.DbTransactionAsync(false);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();
            var context = provider.GetRequiredService<TestDbContext>();
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await service.DbTransactionAsync(true);
                Assert.True(false);
            }
            catch
            {
                Assert.True(true);
            }
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();
            var context = provider.GetRequiredService<TestDbContext>();
            using var transaction = await context.Database.BeginTransactionAsync();
            var dto = await service.DbTransactionAsync<UserDto>(false);
            Assert.NotNull(dto);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IContextRepo>();
            var context = provider.GetRequiredService<TestDbContext>();
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var dto = await service.DbTransactionAsync<UserDto>(true);
                Assert.True(false);
            }
            catch
            {
                Assert.True(true);
            }
        }
    }


}
