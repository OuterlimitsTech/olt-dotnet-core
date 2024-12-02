using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Models;
using OLT.EF.Core.Services.Tests.Lib.Abstract;

namespace OLT.EF.Core.Services.Tests.Automapper;

public class EntityIdServiceTests : BaseUnitTests
{

    [Fact]
    public async Task Add()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var model = PersonModel.FakerEntity();
            var result = await service.AddAsync(model);
            result.Should().BeEquivalentTo(model, opt => opt.Excluding(t => t.PersonId));

            model = PersonModel.FakerEntity();
            var dto = await service.AddAsync<PersonDto, PersonModel>(model);
            Assert.Equal(model.Name.First, dto.First);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var model = PersonModel.FakerEntity();
            var result = service.Add(model);
            result.Should().BeEquivalentTo(model, opt => opt.Excluding(t => t.PersonId));

            model = PersonModel.FakerEntity();
            var dto = service.Add<PersonDto, PersonModel>(model);
            Assert.Equal(model.Name.First, dto.First);
        }
    }


    [Fact]
    public async Task AddList()
    {

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var list = new List<PersonModel>
                {
                    PersonModel.FakerEntity(),
                    PersonModel.FakerEntity(),
                    PersonModel.FakerEntity()
                };
            service.Add(list).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.PersonId));
            service.Add(list.ToArray()).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.PersonId));
            service.Add(list.AsEnumerable()).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.PersonId));

            var dtoList = list.Select(s => new PersonDto
            {
                UniqueId = s.UniqueId,
                First = s.Name.First,
                Middle = s.Name.Middle,
                Last = s.Name.Last,
                Suffix = s.Name.Suffix,
            });

            service.Add<PersonDto, PersonModel>(list).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.PersonId));
            service.Add<PersonDto, PersonModel>(list.ToArray()).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.PersonId));
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var list = new List<PersonModel>
                {
                    PersonModel.FakerEntity(),
                    PersonModel.FakerEntity(),
                    PersonModel.FakerEntity()
                };
            (await service.AddAsync(list)).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.PersonId));
            (await service.AddAsync(list.ToArray())).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.PersonId));
            (await service.AddAsync(list.AsEnumerable())).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.PersonId));

            var dtoList = list.Select(s => new PersonDto
            {
                UniqueId = s.UniqueId,
                First = s.Name.First,
                Middle = s.Name.Middle,
                Last = s.Name.Last,
                Suffix = s.Name.Suffix,
            });

            (await service.AddAsync<PersonDto, PersonModel>(list)).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.PersonId));
            (await service.AddAsync<PersonDto, PersonModel>(list.ToArray())).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.PersonId));
        }
    }


    [Fact]
    public async Task Update()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var addModel = PersonDto.FakerEntity();
            var model = service.Add(addModel);
            model.First = Faker.Name.First();
            var updated = service.Update<PersonModel, PersonDto>(model.PersonId.GetValueOrDefault(), model);
            Assert.Equal(model.First, updated.Name.First);
            Assert.NotEqual(addModel.First, updated.Name.First);

            model.First = Faker.Name.First();
            var dto = service.Update(model.PersonId.GetValueOrDefault(), model);
            Assert.Equal(model.First, dto.First);

            model.First = Faker.Name.First();
            dto = service.Update(new OltSearcherGetById<PersonEntity>(model.PersonId.GetValueOrDefault()), model);
            Assert.Equal(model.First, dto.First);

            model.First = Faker.Name.First();
            updated = service.Update<PersonModel, PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.GetValueOrDefault()), model);
            Assert.Equal(model.First, updated.Name.First);

        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var addModel = PersonDto.FakerEntity();
            var model = service.Add(addModel);

            model.First = Faker.Name.First();
            var updated = await service.UpdateAsync<PersonModel, PersonDto>(model.PersonId.GetValueOrDefault(), model);
            Assert.Equal(model.First, updated.Name.First);
            Assert.NotEqual(addModel.First, updated.Name.First);

            model.First = Faker.Name.First();
            var dto = await service.UpdateAsync(model.PersonId.GetValueOrDefault(), model);
            Assert.Equal(model.First, dto.First);

            model.First = Faker.Name.First();
            dto = await service.UpdateAsync(new OltSearcherGetById<PersonEntity>(model.PersonId.GetValueOrDefault()), model);
            Assert.Equal(model.First, dto.First);


            model.First = Faker.Name.First();
            updated = await service.UpdateAsync<PersonModel, PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.GetValueOrDefault()), model);
            Assert.Equal(model.First, updated.Name.First);
        }
    }

    [Fact]
    public async Task AddUpdateWithChildEntity()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var addModel = PersonWithAddressDto.FakerDto(4);
            var model = service.Add(addModel);

            Assert.Equal(4, model.Addresses.Count);

            var address = model.Addresses.First();
            model.First = Faker.Name.First();
            model.Addresses[1].Street = "1234 Oak";


            var updated = service.Update<PersonWithAddressDto>(model.PersonId.GetValueOrDefault(), model, queryable => queryable.Include(i => i.Addresses));

            Assert.Equal(4, updated.Addresses.Count);
            Assert.Equal(model.Addresses[1].Street, updated.Addresses[1].Street);


            //NOTE:  The inmemory provider always does the include UGH!
            //model = service.Get<PersonWithAddressDto>(model.PersonId.Value);
            //updated = service.Update<PersonWithAddressDto>(model.PersonId.GetValueOrDefault(), model);  //Without the include, it will automatically re-add all addresses
            //Assert.Equal(8, updated.Addresses.Count);

        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var addModel = PersonWithAddressDto.FakerDto(4);
            var model = await service.AddAsync(addModel);

            Assert.Equal(4, model.Addresses.Count);

            var address = model.Addresses.First();
            model.First = Faker.Name.First();
            model.Addresses[1].Street = "1234 Oak";


            var updated = await service.UpdateAsync<PersonWithAddressDto>(model.PersonId.GetValueOrDefault(), model, queryable => queryable.Include(i => i.Addresses));

            Assert.Equal(4, updated.Addresses.Count);
            Assert.Equal(model.Addresses[1].Street, updated.Addresses[1].Street);
        }
    }

    [Fact]
    public async Task SoftDelete()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var model = await service.AddAsync(PersonDto.FakerEntity());
            Assert.True(await service.SoftDeleteAsync(model.PersonId.Value));
            Assert.False(await service.SoftDeleteAsync(-1000));
            model = service.Add(PersonDto.FakerEntity());
            Assert.True(await service.SoftDeleteAsync(new OltSearcherGetById<PersonEntity>(model.PersonId.Value)));
            Assert.False(await service.SoftDeleteAsync(new OltSearcherGetByUid<PersonEntity>(Guid.NewGuid())));

            Assert.NotNull(await service.GetAsync<PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.Value, true)));
            Assert.Null(await service.GetAsync<PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.Value)));
            Assert.Null(await service.GetAsync<PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.Value, false)));

            //Without Searcher
            Assert.NotNull(await service.GetAsync<PersonDto>(model.PersonId.Value, true));
            Assert.Null(await service.GetAsync<PersonDto>(model.PersonId.Value));
            Assert.Null(await service.GetAsync<PersonDto>(model.PersonId.Value, false));

            //Get Safe
            Assert.NotNull(await service.GetSafeAsync<PersonDto>(model.PersonId.Value, true));
            await Assert.ThrowsAsync<OltRecordNotFoundException>(() => service.GetSafeAsync<PersonDto>(model.PersonId.Value, false));

        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var model = service.Add(PersonDto.FakerEntity());
            Assert.True(service.SoftDelete(model.PersonId.Value));
            Assert.False(service.SoftDelete(-1000));
            model = service.Add(PersonDto.FakerEntity());
            Assert.True(service.SoftDelete(new OltSearcherGetById<PersonEntity>(model.PersonId.Value)));
            Assert.False(service.SoftDelete(new OltSearcherGetByUid<PersonEntity>(Guid.NewGuid())));

            Assert.NotNull(service.Get<PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.Value, true)));
            Assert.Null(service.Get<PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.Value)));
            Assert.Null(service.Get<PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.Value, false)));

            //Without Searcher
            Assert.NotNull(service.Get<PersonDto>(model.PersonId.Value, true));
            Assert.Null(service.Get<PersonDto>(model.PersonId.Value));
            Assert.Null(service.Get<PersonDto>(model.PersonId.Value, false));


            //Get Safe
            Assert.NotNull(service.GetSafe<PersonDto>(model.PersonId.Value, true));
            Assert.Throws<OltRecordNotFoundException>(() => service.GetSafe<PersonDto>(model.PersonId.Value, false));


        }
    }

    [Fact]
    public async Task Get()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var expected = service.Add(PersonDto.FakerEntity());
            var subject = await service.GetAsync<PersonDto>(expected.PersonId.GetValueOrDefault());
            subject.Should().BeEquivalentTo(expected);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var expected = service.Add(PersonDto.FakerEntity());
            var subject = await service.GetSafeAsync<PersonDto>(expected.PersonId.GetValueOrDefault());
            subject.Should().BeEquivalentTo(expected);

            await Assert.ThrowsAsync<OltRecordNotFoundException>(() => service.GetSafeAsync<PersonDto>(Faker.RandomNumber.Next(-100, -1), false));
            await Assert.ThrowsAsync<OltRecordNotFoundException>(() => service.GetSafeAsync<PersonDto>(Faker.RandomNumber.Next(-100, -1), true));
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var expected = service.Add(PersonDto.FakerEntity());
            var subject = service.Get<PersonDto>(expected.PersonId.GetValueOrDefault());
            subject.Should().BeEquivalentTo(expected);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var expected = service.Add(PersonDto.FakerEntity());
            var subject = service.GetSafe<PersonDto>(expected.PersonId.GetValueOrDefault());
            subject.Should().BeEquivalentTo(expected);

            Assert.Throws<OltRecordNotFoundException>(() => service.GetSafe<PersonDto>(Faker.RandomNumber.Next(-100, -1), false));
            Assert.Throws<OltRecordNotFoundException>(() => service.GetSafe<PersonDto>(Faker.RandomNumber.Next(-100, -1), true));
        }

    }

    [Fact]
    public async Task GetAll()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var expected = new List<PersonModel>();
            var filterIds = new List<int>();
            for (var idx = 0; idx <= 5; idx++)
            {
                var model = await service.AddAsync(PersonModel.FakerEntity());
                expected.Add(model);
                filterIds.Add(model.PersonId.GetValueOrDefault());
            }

            var records = await service.GetAllAsync<PersonModel>(new OltSearcherGetAll<PersonEntity>());

            records
                .Where(p => filterIds.Contains(p.PersonId.GetValueOrDefault()))
                .OrderBy(p => p.PersonId)
                .Should()
                .BeEquivalentTo(expected.OrderBy(p => p.PersonId));
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var expected = new List<PersonModel>();
            var filterIds = new List<int>();
            for (var idx = 0; idx <= 5; idx++)
            {
                var model = service.Add(PersonModel.FakerEntity());
                expected.Add(model);
                filterIds.Add(model.PersonId.GetValueOrDefault());
            }
            service.GetAll<PersonModel>(new OltSearcherGetAll<PersonEntity>())
                .Where(p => filterIds.Contains(p.PersonId.GetValueOrDefault()))
                .OrderBy(p => p.PersonId)
                .Should()
                .BeEquivalentTo(expected.OrderBy(p => p.PersonId));

        }


    }


    [Fact]
    public async Task GetPaged()
    {
        var pagedParams = new OltPagingParams { Page = 2, Size = 25 };

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonUniqueIdService>();
            var people = new List<PersonDto>();
            for (var idx = 0; idx <= 115; idx++)
            {
                people.Add(service.Add<PersonDto, PersonModel>(PersonModel.FakerEntity()));
            }
            var paged1 = await service.GetPagedAsync<PersonDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams);
            paged1.Should().BeEquivalentTo(people.OrderBy(p => p.Last).ThenBy(p => p.First).ThenBy(p => p.PersonId).AsQueryable().ToPaged(pagedParams), opt => opt.WithStrictOrdering());

            var paged2 = service.GetPaged<PersonDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams);
            paged2.Should().BeEquivalentTo(people.OrderBy(p => p.Last).ThenBy(p => p.First).ThenBy(p => p.PersonId).AsQueryable().ToPaged(pagedParams), opt => opt.WithStrictOrdering());

            var paged3 = await service.GetPagedAsync<PersonDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams, orderBy => orderBy.OrderBy(p => p.NameFirst).ThenBy(p => p.NameLast).ThenBy(p => p.Id));
            paged3.Should().BeEquivalentTo(people.OrderBy(p => p.First).ThenBy(p => p.Last).ThenBy(p => p.PersonId).AsQueryable().ToPaged(pagedParams), opt => opt.WithStrictOrdering());

            var paged4 = service.GetPaged<PersonDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams, orderBy => orderBy.OrderBy(p => p.NameFirst).ThenBy(p => p.NameLast).ThenBy(p => p.Id));
            paged4.Should().BeEquivalentTo(people.OrderBy(p => p.First).ThenBy(p => p.Last).ThenBy(p => p.PersonId).AsQueryable().ToPaged(pagedParams), opt => opt.WithStrictOrdering());
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonUniqueIdService>();
            Assert.Throws<OltAdapterNotFoundException>(() => service.GetPaged<UserDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams, orderBy => orderBy.OrderBy(p => p.NameFirst).ThenBy(p => p.NameLast).ThenBy(p => p.Id)));
            await Assert.ThrowsAsync<OltAdapterNotFoundException>(() => service.GetPagedAsync<UserDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams, orderBy => orderBy.OrderBy(p => p.NameFirst).ThenBy(p => p.NameLast).ThenBy(p => p.Id)));
        }
    }


    [Fact]
    public async Task GetByIdSearcher()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var newDto = await service.AddAsync(PersonModel.FakerEntity());
            var result = await service.GetAsync<PersonDto>(new OltSearcherGetById<PersonEntity>(newDto.PersonId.GetValueOrDefault()));
            Assert.Equal(newDto.PersonId, result?.PersonId);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var newDto = service.Add(PersonModel.FakerEntity());
            var result = service.Get<PersonDto>(new OltSearcherGetById<PersonEntity>(newDto.PersonId.GetValueOrDefault()));
            Assert.Equal(newDto.PersonId, result?.PersonId);
        }


        // ***************** Deleted Records ******************
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var newDto = await service.AddAsync(PersonModel.FakerEntity());
            await service.SoftDeleteAsync(newDto.PersonId.GetValueOrDefault());
            var result = await service.GetAsync<PersonDto>(new OltSearcherGetById<PersonEntity>(newDto.PersonId.GetValueOrDefault(), true));
            Assert.Equal(newDto.PersonId, result?.PersonId);

            var nullResult = await service.GetAsync<PersonDto>(new OltSearcherGetById<PersonEntity>(newDto.PersonId.GetValueOrDefault(), false));
            Assert.Null(nullResult);
        }


        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var newDto = service.Add(PersonModel.FakerEntity());
            service.SoftDelete(newDto.PersonId.GetValueOrDefault());
            var result = service.Get<PersonDto>(new OltSearcherGetById<PersonEntity>(newDto.PersonId.GetValueOrDefault()));
            Assert.Null(result);
        }


    }

    [Fact]
    public async Task GetByAllSearcher()
    {

        // ***************** Exclude Deleted Records ******************
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var newDto = await service.AddAsync(PersonModel.FakerEntity());
            service.SoftDelete(newDto.PersonId.GetValueOrDefault());
            var result = service.GetAll<PersonDto>(new OltSearcherGetAll<PersonEntity>()).FirstOrDefault(p => p.PersonId == newDto.PersonId);
            Assert.Null(result);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var newDto = await service.AddAsync(PersonModel.FakerEntity());
            await service.SoftDeleteAsync(newDto.PersonId.GetValueOrDefault());
            var results = await service.GetAllAsync<PersonDto>(new OltSearcherGetAll<PersonEntity>());
            var result = results.FirstOrDefault(p => p.PersonId == newDto.PersonId);
            Assert.Null(result);
        }


        // ***************** Include Deleted Records ******************
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var newDto = await service.AddAsync(PersonModel.FakerEntity());
            await service.SoftDeleteAsync(newDto.PersonId.GetValueOrDefault());
            var results = await service.GetAllAsync<PersonDto>(new OltSearcherGetAll<PersonEntity>(true));
            var result = results.FirstOrDefault(p => p.PersonId == newDto.PersonId);
            Assert.Equal(newDto.PersonId, result?.PersonId);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var newDto = await service.AddAsync(PersonModel.FakerEntity());
            service.SoftDelete(newDto.PersonId.GetValueOrDefault());
            var result = service.GetAll<PersonDto>(new OltSearcherGetAll<PersonEntity>(true)).FirstOrDefault(p => p.PersonId == newDto.PersonId);
            Assert.Equal(newDto.PersonId, result?.PersonId);
        }
    }



    [Fact]
    public async Task Count()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var person = await service.AddAsync(PersonModel.FakerEntity());
            Assert.Equal(1, await service.CountAsync(new OltSearcherGetById<PersonEntity>(person.PersonId.Value)));
            Assert.Equal(1, await service.CountAsync(p => p.Id == person.PersonId.Value));
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var person = await service.AddAsync(PersonModel.FakerEntity());
            Assert.Equal(1, service.Count(new OltSearcherGetById<PersonEntity>(person.PersonId.Value)));
            Assert.Equal(1, service.Count(p => p.Id == person.PersonId.Value));
        }
    }



    [Fact]
    public async Task GetByUidSearcher()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var model = PersonModel.FakerEntity();
            await service.AddAsync(model);
            var result = await service.GetAsync<PersonModel>(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.GetValueOrDefault()));
            result.Should().BeEquivalentTo(model, opt => opt.Excluding(t => t.PersonId));
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var model = PersonModel.FakerEntity();
            await service.AddAsync(model);
            var result = service.Get<PersonModel>(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.GetValueOrDefault()));
            result.Should().BeEquivalentTo(model, opt => opt.Excluding(t => t.PersonId));
        }


        // Delete
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var model = PersonModel.FakerEntity();
            await service.AddAsync(model);
            service.SoftDelete(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.GetValueOrDefault()));

            Assert.NotNull(await service.GetAsync<PersonDto>(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.GetValueOrDefault())));
            Assert.Null(await service.GetAsync<PersonDto>(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.GetValueOrDefault(), false)));
        }

    }

    [Fact]
    public async Task Any()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var model = await service.AddAsync(PersonDto.FakerEntity());
            Assert.True(await service.AnyAsync(model.PersonId.Value));
            Assert.True(await service.AnyAsync(p => p.Id == model.PersonId.Value));

            Assert.False(await service.AnyAsync(-1000));
            Assert.False(await service.AnyAsync(p => p.Id == -1000));

            model = service.Add(PersonDto.FakerEntity());
            Assert.True(await service.AnyAsync(new OltSearcherGetById<PersonEntity>(model.PersonId.Value)));
            Assert.False(await service.AnyAsync(new OltSearcherGetByUid<PersonEntity>(Guid.NewGuid())));
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var model = service.Add(PersonDto.FakerEntity());
            Assert.True(service.Any(model.PersonId.Value));
            Assert.True(service.Any(p => p.Id == model.PersonId.Value));

            Assert.False(service.Any(-1000));
            Assert.False(service.Any(p => p.Id == -1000));

            model = service.Add(PersonDto.FakerEntity());
            Assert.True(service.Any(new OltSearcherGetById<PersonEntity>(model.PersonId.Value)));
            Assert.False(service.Any(new OltSearcherGetByUid<PersonEntity>(Guid.NewGuid())));
        }
    }


    [Fact]
    public async Task GetSafeTests()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var expected = service.Add(PersonDto.FakerEntity());
            var subject = await service.GetSafeTestAsync<PersonDto>(expected.PersonId.GetValueOrDefault());
            subject.Should().BeEquivalentTo(expected);


            Func<Task> action = async () => await service.GetSafeTestAsync<PersonDto>(Faker.RandomNumber.Next(-100, -50));
            await action.Should().ThrowAsync<OltRecordNotFoundException>();
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IPersonService>();

            var expected = service.Add(PersonDto.FakerEntity());
            var subject = service.Get<PersonDto>(expected.PersonId.GetValueOrDefault());
            subject.Should().BeEquivalentTo(expected);


            Action action = () => service.GetSafeTest<PersonDto>(Faker.RandomNumber.Next(-100, -50));
            action.Should().Throw<OltRecordNotFoundException>();
        }

    }
}
