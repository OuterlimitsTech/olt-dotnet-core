using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Models;
using OLT.EF.Core.Services.Tests.Assets.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OLT.EF.Core.Services.Tests
{
    public class EntityIdServiceTests : BaseUnitTests
    {

        [Fact]
        public async Task Add()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonService>();

                var model = PersonAutoMapperModel.FakerEntity();
                var result = await service.AddAsync(model);
                result.Should().BeEquivalentTo(model, opt => opt.Excluding(t => t.PersonId));

                model = PersonAutoMapperModel.FakerEntity();
                var dto = await service.AddAsync<PersonDto, PersonAutoMapperModel>(model);
                Assert.Equal(model.Name.First, dto.First);
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonService>();

                var model = PersonAutoMapperModel.FakerEntity();
                var result = service.Add(model);
                result.Should().BeEquivalentTo(model, opt => opt.Excluding(t => t.PersonId));

                model = PersonAutoMapperModel.FakerEntity();
                var dto = service.Add<PersonDto, PersonAutoMapperModel>(model);
                Assert.Equal(model.Name.First, dto.First);
            }
        }


        [Fact]
        public async Task AddList()
        {

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonService>();

                var list = new List<PersonAutoMapperModel>
                {
                    PersonAutoMapperModel.FakerEntity(),
                    PersonAutoMapperModel.FakerEntity(),
                    PersonAutoMapperModel.FakerEntity()
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

                service.Add<PersonDto, PersonAutoMapperModel>(list).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.PersonId));
                service.Add<PersonDto, PersonAutoMapperModel>(list.ToArray()).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.PersonId));
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonService>();

                var list = new List<PersonAutoMapperModel>
                {
                    PersonAutoMapperModel.FakerEntity(),
                    PersonAutoMapperModel.FakerEntity(),
                    PersonAutoMapperModel.FakerEntity()
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

                (await service.AddAsync<PersonDto, PersonAutoMapperModel>(list)).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.PersonId));
                (await service.AddAsync<PersonDto, PersonAutoMapperModel>(list.ToArray())).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.PersonId));
            } 
        }

        //[Fact]
        //public void UpdateAutoMapper()
        //{
        //    var addModel = UnitTestHelper.CreatePersonDto();
        //    var model = service.Add(addModel);
        //    model.First = Faker.Name.First();
        //    var updated = service.Update(model.PersonId.GetValueOrDefault(), model);
        //    updated.Should().BeEquivalentTo(model);
        //    Assert.True(updated.First.Equals(model.First) && !addModel.First.Equals(updated.First));
        //}


        [Fact]
        public async Task Update()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonService>();

                var addModel = PersonDto.FakerEntity();
                var model = service.Add(addModel);
                model.First = Faker.Name.First();
                var updated = service.Update<PersonAutoMapperModel, PersonDto>(model.PersonId.GetValueOrDefault(), model);
                Assert.Equal(model.First, updated.Name.First);
                Assert.NotEqual(addModel.First, updated.Name.First);

                model.First = Faker.Name.First();
                var dto = service.Update(model.PersonId.GetValueOrDefault(), model);
                Assert.Equal(model.First, dto.First);

                model.First = Faker.Name.First();
                dto = service.Update(new OltSearcherGetById<PersonEntity>(model.PersonId.GetValueOrDefault()), model);
                Assert.Equal(model.First, dto.First);
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonService>();

                var addModel = PersonDto.FakerEntity();
                var model = service.Add(addModel);
                model.First = Faker.Name.First();
                var updated = await service.UpdateAsync<PersonAutoMapperModel, PersonDto>(model.PersonId.GetValueOrDefault(), model);
                Assert.Equal(model.First, updated.Name.First);
                Assert.NotEqual(addModel.First, updated.Name.First);

                model.First = Faker.Name.First();
                var dto = await service.UpdateAsync(model.PersonId.GetValueOrDefault(), model);
                Assert.Equal(model.First, dto.First);

                model.First = Faker.Name.First();
                dto = await service.UpdateAsync(new OltSearcherGetById<PersonEntity>(model.PersonId.GetValueOrDefault()), model);
                Assert.Equal(model.First, dto.First);
            }
        }


        //[Fact]
        //public void SoftDelete()
        //{
        //    var model = service.Add(UnitTestHelper.CreatePersonDto());
        //    Assert.True(service.SoftDelete(model.PersonId.Value));
        //    Assert.False(service.SoftDelete(-1000));
        //    model = service.Add(UnitTestHelper.CreatePersonDto());
        //    Assert.True(service.SoftDelete(new OltSearcherGetById<PersonEntity>(model.PersonId.Value)));
        //    Assert.False(service.SoftDelete(new OltSearcherGetByUid<PersonEntity>(Guid.NewGuid())));

        //    Assert.NotNull(service.Get<PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.Value)));
        //    Assert.Null(service.Get<PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.Value, false)));
        //}


        //[Fact]
        //public async Task SoftDeleteAsync()
        //{
        //    var model = await service.AddAsync(UnitTestHelper.CreatePersonDto());
        //    Assert.True(await service.SoftDeleteAsync(model.PersonId.Value));
        //    Assert.False(await service.SoftDeleteAsync(-1000));
        //    model = service.Add(UnitTestHelper.CreatePersonDto());
        //    Assert.True(await service.SoftDeleteAsync(new OltSearcherGetById<PersonEntity>(model.PersonId.Value)));
        //    Assert.False(await service.SoftDeleteAsync(new OltSearcherGetByUid<PersonEntity>(Guid.NewGuid())));

        //    Assert.NotNull(await service.GetAsync<PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.Value)));
        //    Assert.Null(await service.GetAsync<PersonDto>(new OltSearcherGetById<PersonEntity>(model.PersonId.Value, false)));

        //}

        //[Fact]
        //public void Get()
        //{
        //    var expected = service.Add(UnitTestHelper.CreatePersonDto());
        //    var subject = service.Get<PersonDto>(expected.PersonId.GetValueOrDefault());
        //    subject.Should().BeEquivalentTo(expected);
        //}

        //[Fact]
        //public async Task GetAsync()
        //{
        //    var expected = service.Add(UnitTestHelper.CreatePersonDto());
        //    var subject = await service.GetAsync<PersonDto>(expected.PersonId.GetValueOrDefault());
        //    subject.Should().BeEquivalentTo(expected);
        //}


        //[Fact]
        //public void GetAll()
        //{
        //    var expected = new List<PersonAutoMapperModel>();
        //    var filterIds = new List<int>();
        //    for (var idx = 0; idx <= 5; idx++)
        //    {
        //        var model = service.Add(UnitTestHelper.CreateTestAutoMapperModel());
        //        expected.Add(model);
        //        filterIds.Add(model.PersonId.GetValueOrDefault());
        //    }
        //    var records = service.GetAll<PersonAutoMapperModel>(new OltSearcherGetAll<PersonEntity>()).Where(p => filterIds.Contains(p.PersonId.GetValueOrDefault())).OrderBy(p => p.PersonId).ToList();
        //    records.Should().BeEquivalentTo(expected.OrderBy(p => p.PersonId));
        //}

        //[Fact]
        //public async Task GetAllAsync()
        //{
        //    var expected = new List<PersonAutoMapperModel>();
        //    var filterIds = new List<int>();
        //    for (var idx = 0; idx <= 5; idx++)
        //    {
        //        var model = await service.AddAsync(UnitTestHelper.CreateTestAutoMapperModel());
        //        expected.Add(model);
        //        filterIds.Add(model.PersonId.GetValueOrDefault());
        //    }

        //    var records = await service.GetAllAsync<PersonAutoMapperModel>(new OltSearcherGetAll<PersonEntity>());

        //    records
        //        .Where(p => filterIds.Contains(p.PersonId.GetValueOrDefault()))
        //        .OrderBy(p => p.PersonId)
        //        .Should()
        //        .BeEquivalentTo(expected.OrderBy(p => p.PersonId));
        //}

        //[Fact]
        //public void GetPaged()
        //{
        //    for (var idx = 0; idx <= 500; idx++)
        //    {
        //        service.Add(UnitTestHelper.CreateTestAutoMapperModel());
        //    }
        //    var pagedParams = new OltPagingParams { Page = 2, Size = 25 };
        //    var paged = service.GetPaged<PersonDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams);
        //    Assert.True(paged.Data.Count() == pagedParams.Size && paged.Page == pagedParams.Page && paged.Size == pagedParams.Size);
        //}

        //[Fact]
        //public async Task GetPagedAsync()
        //{
        //    for (var idx = 0; idx <= 500; idx++)
        //    {
        //        service.Add(UnitTestHelper.CreateTestAutoMapperModel());
        //    }
        //    var pagedParams = new OltPagingParams { Page = 2, Size = 25 };
        //    var paged = await service.GetPagedAsync<PersonDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams);
        //    Assert.True(paged.Data.Count() == pagedParams.Size && paged.Page == pagedParams.Page && paged.Size == pagedParams.Size);
        //}

        //[Fact]
        //public void GetPagedAutoMapper()
        //{
        //    for (var idx = 0; idx <= 500; idx++)
        //    {
        //        service.Add(UnitTestHelper.CreateTestAutoMapperModel());
        //    }
        //    var pagedParams = new OltPagingParams { Page = 4, Size = 50 };
        //    var paged = service.GetPaged<PersonAutoMapperPagedDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams);
        //    Assert.True(paged.Data.Count() == pagedParams.Size && paged.Page == pagedParams.Page && paged.Size == pagedParams.Size);
        //}

        //[Fact]
        //public async Task GetPagedAutoMapperAsync()
        //{
        //    for (var idx = 0; idx <= 500; idx++)
        //    {
        //        service.Add(UnitTestHelper.CreateTestAutoMapperModel());
        //    }
        //    var pagedParams = new OltPagingParams { Page = 4, Size = 50 };
        //    var paged = await service.GetPagedAsync<PersonAutoMapperPagedDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams);
        //    Assert.True(paged.Data.Count() == pagedParams.Size && paged.Page == pagedParams.Page && paged.Size == pagedParams.Size);
        //}

        //[Fact]
        //public void GetByIdSearcherDeleted()
        //{
        //    var newDto = service.Add(UnitTestHelper.CreateTestAutoMapperModel());
        //    service.SoftDelete(newDto.PersonId.GetValueOrDefault());
        //    var result = service.Get<PersonDto>(new OltSearcherGetById<PersonEntity>(newDto.PersonId.GetValueOrDefault()));
        //    Assert.Equal(newDto.PersonId, result?.PersonId);
        //}

        //[Fact]
        //public async Task GetByIdSearcherDeletedAsync()
        //{
        //    var newDto = await service.AddAsync(UnitTestHelper.CreateTestAutoMapperModel());
        //    await service.SoftDeleteAsync(newDto.PersonId.GetValueOrDefault());
        //    var result = await service.GetAsync<PersonDto>(new OltSearcherGetById<PersonEntity>(newDto.PersonId.GetValueOrDefault()));
        //    Assert.Equal(newDto.PersonId, result?.PersonId);
        //}

        //[Fact]
        //public void GetByIdSearcher()
        //{
        //    var newDto = service.Add(UnitTestHelper.CreateTestAutoMapperModel());
        //    var result = service.Get<PersonDto>(new OltSearcherGetById<PersonEntity>(newDto.PersonId.GetValueOrDefault()));
        //    Assert.Equal(newDto.PersonId, result?.PersonId);
        //}

        //[Fact]
        //public async Task GetByIdSearcherAsync()
        //{
        //    var newDto = await service.AddAsync(UnitTestHelper.CreateTestAutoMapperModel());
        //    var result = await service.GetAsync<PersonDto>(new OltSearcherGetById<PersonEntity>(newDto.PersonId.GetValueOrDefault()));
        //    Assert.Equal(newDto.PersonId, result?.PersonId);
        //}

        //[Fact]
        //public void GetByAllSearcherIncludeDeleted()
        //{
        //    var newDto = service.Add(UnitTestHelper.CreateTestAutoMapperModel());
        //    service.SoftDelete(newDto.PersonId.GetValueOrDefault());
        //    var result = service.GetAll<PersonDto>(new OltSearcherGetAll<PersonEntity>(true)).FirstOrDefault(p => p.PersonId == newDto.PersonId);
        //    Assert.Equal(newDto.PersonId, result?.PersonId);
        //}

        //[Fact]
        //public async Task GetByAllSearcherIncludeDeletedAsync()
        //{
        //    var newDto = await service.AddAsync(UnitTestHelper.CreateTestAutoMapperModel());
        //    await service.SoftDeleteAsync(newDto.PersonId.GetValueOrDefault());
        //    var results = await service.GetAllAsync<PersonDto>(new OltSearcherGetAll<PersonEntity>(true));
        //    var result = results.FirstOrDefault(p => p.PersonId == newDto.PersonId);
        //    Assert.Equal(newDto.PersonId, result?.PersonId);
        //}

        //[Fact]
        //public void GetByAllSearcherExcludeDeleted()
        //{
        //    var newDto = service.Add(UnitTestHelper.CreateTestAutoMapperModel());
        //    service.SoftDelete(newDto.PersonId.GetValueOrDefault());
        //    var result = service.GetAll<PersonDto>(new OltSearcherGetAll<PersonEntity>()).FirstOrDefault(p => p.PersonId == newDto.PersonId);
        //    Assert.Null(result);
        //}

        //[Fact]
        //public async Task GetByAllSearcherExcludeDeletedAsync()
        //{
        //    var newDto = await service.AddAsync(UnitTestHelper.CreateTestAutoMapperModel());
        //    await service.SoftDeleteAsync(newDto.PersonId.GetValueOrDefault());
        //    var results = await service.GetAllAsync<PersonDto>(new OltSearcherGetAll<PersonEntity>());
        //    var result = results.FirstOrDefault(p => p.PersonId == newDto.PersonId);
        //    Assert.Null(result);
        //}

        //[Fact]
        //public void Count()
        //{
        //    var person = service.Add(UnitTestHelper.CreateTestAutoMapperModel());
        //    Assert.Equal(1, service.Count(new OltSearcherGetById<PersonEntity>(person.PersonId.Value)));
        //}

        //[Fact]
        //public async Task CountAsync()
        //{
        //    var person = await service.AddAsync(UnitTestHelper.CreateTestAutoMapperModel());
        //    Assert.Equal(1, await service.CountAsync(new OltSearcherGetById<PersonEntity>(person.PersonId.Value)));
        //}
    }
}