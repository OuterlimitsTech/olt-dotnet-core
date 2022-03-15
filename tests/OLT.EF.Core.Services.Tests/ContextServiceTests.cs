using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Searchers;
using OLT.EF.Core.Services.Tests.Assets.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OLT.EF.Core.Services.Tests
{

    public class ContextServiceTests : BaseUnitTests
    {

        [Fact]
        public void QueryableSearchers()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IContextService>();
                var model = service.CreatePerson();
                var result = service.Get(false, new PersonFirstNameStartsWithSearcher(model.NameFirst), new PersonLastNameStartsWithSearcher(model.NameLast));
                Assert.Equal(model.Id, result.Id);
            }
        }


        [Fact]
        public void QueryableSearcher()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IContextService>();
                var model = service.CreatePerson();
                var result = service.Get(new OltSearcherGetById<PersonEntity>(model.Id));
                Assert.Equal(model.Id, result.Id);
            }
        }

        [Fact]
        public void Get()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IContextService>();

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
                Assert.NotNull(service.GetDtoUser(userEntity.Id));
            }
        }


        [Fact]
        public async Task GetAsync()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IContextService>();
                var personEntity = service.CreatePerson();
                var userEntity = service.CreateUser();
                Assert.True((await service.GetAllPeopleAsync()).Any());
                Assert.True((await service.GetAllPeopleSearcherAsync()).Any());
                Assert.True((await service.GetAllUsersAsync()).Any());
                Assert.True((await service.GetAllUsersOrderedAsync()).Any());
                Assert.True((await service.GetAllUsersSearcherAsync()).Any());
                Assert.True((await service.GetAllDtoUsersAsync()).Any());
                Assert.True((await service.GetAllDtoUsersSearcherAsync()).Any());
                Assert.NotNull(await service.GetDtoUserAsync(userEntity.Id));
            }
        }


        [Fact]
        public void DeletedTest()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IContextService>();

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
                var service = provider.GetService<IContextService>();

                var personEntity = service.CreatePerson();
                Assert.NotNull(service.Get(false).FirstOrDefault(p => p.Id == personEntity.Id));
                Assert.NotNull(service.Get(true).FirstOrDefault(p => p.Id == personEntity.Id));
                Assert.NotNull(service.GetNonDeleted().FirstOrDefault(p => p.Id == personEntity.Id));
                Assert.True(await service.DeleteAsync<PersonEntity>(personEntity.Id));
                Assert.Null(service.Get(false).FirstOrDefault(p => p.Id == personEntity.Id));
                Assert.NotNull(service.Get(true).FirstOrDefault(p => p.Id == personEntity.Id));
                Assert.Null(service.GetNonDeleted().FirstOrDefault(p => p.Id == personEntity.Id));

                var userEntity = service.CreateUser();
                Assert.Equal(userEntity.Id, (await service.GetDtoUserAsync(userEntity.Id))?.UserId);
                await Assert.ThrowsAsync<InvalidCastException>(() => service.DeleteAsync<UserEntity>(userEntity.Id));
            }


        }
    }
}