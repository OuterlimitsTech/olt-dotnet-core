using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Models;
using OLT.EF.Core.Services.Tests.Assets.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OLT.EF.Core.Services.Tests
{
    public class EntityUidServiceTests : BaseUnitTests
    {

        [Fact]
        public async Task Add()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var model = PersonAutoMapperModel.FakerEntity();
                var result = await service.AddAsync(model);
                result.Should().BeEquivalentTo(model, opt => opt.Excluding(t => t.PersonId));

                model = PersonAutoMapperModel.FakerEntity();
                var dto = await service.AddAsync<PersonDto, PersonAutoMapperModel>(model);
                Assert.Equal(model.Name.First, dto.First);
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

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
                var service = provider.GetService<IPersonUniqueIdService>();

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
                var service = provider.GetService<IPersonUniqueIdService>();

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


        [Fact]
        public async Task Update()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var addModel = PersonDto.FakerEntity();
                var model = service.Add(addModel);
                model.First = Faker.Name.First();
                var updated = service.Update<PersonAutoMapperModel, PersonDto>(model.UniqueId.GetValueOrDefault(), model);
                Assert.Equal(model.First, updated.Name.First);
                Assert.NotEqual(addModel.First, updated.Name.First);

                model.First = Faker.Name.First();
                var dto = service.Update(model.UniqueId.GetValueOrDefault(), model);
                Assert.Equal(model.First, dto.First);

                model.First = Faker.Name.First();
                dto = service.Update(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.GetValueOrDefault()), model);
                Assert.Equal(model.First, dto.First);
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var addModel = PersonDto.FakerEntity();
                var model = service.Add(addModel);
                model.First = Faker.Name.First();
                var updated = await service.UpdateAsync<PersonAutoMapperModel, PersonDto>(model.UniqueId.GetValueOrDefault(), model);
                Assert.Equal(model.First, updated.Name.First);
                Assert.NotEqual(addModel.First, updated.Name.First);

                model.First = Faker.Name.First();
                var dto = await service.UpdateAsync(model.UniqueId.GetValueOrDefault(), model);
                Assert.Equal(model.First, dto.First);

                model.First = Faker.Name.First();
                dto = await service.UpdateAsync(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.GetValueOrDefault()), model);
                Assert.Equal(model.First, dto.First);
            }
        }

        [Fact]
        public async Task SoftDelete()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var model = await service.AddAsync(PersonDto.FakerEntity());
                Assert.True(await service.SoftDeleteAsync(model.UniqueId.Value));
                Assert.False(await service.SoftDeleteAsync(Guid.NewGuid()));
                model = service.Add(PersonDto.FakerEntity());
                Assert.True(await service.SoftDeleteAsync(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.Value)));
                Assert.False(await service.SoftDeleteAsync(new OltSearcherGetByUid<PersonEntity>(Guid.NewGuid())));

                Assert.NotNull(await service.GetAsync<PersonDto>(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.Value)));
                Assert.Null(await service.GetAsync<PersonDto>(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.Value, false)));
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var model = service.Add(PersonDto.FakerEntity());
                Assert.True(service.SoftDelete(model.UniqueId.Value));
                Assert.False(service.SoftDelete(Guid.NewGuid()));
                model = service.Add(PersonDto.FakerEntity());
                Assert.True(service.SoftDelete(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.Value)));
                Assert.False(service.SoftDelete(new OltSearcherGetByUid<PersonEntity>(Guid.NewGuid())));

                Assert.NotNull(service.Get<PersonDto>(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.Value)));
                Assert.Null(service.Get<PersonDto>(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.Value, false)));
            }
        }

        [Fact]
        public async Task Get()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var expected = service.Add(PersonDto.FakerEntity());
                var subject = await service.GetAsync<PersonDto>(expected.UniqueId.GetValueOrDefault());
                subject.Should().BeEquivalentTo(expected);
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var expected = service.Add(PersonDto.FakerEntity());
                var subject = service.Get<PersonDto>(expected.UniqueId.GetValueOrDefault());
                subject.Should().BeEquivalentTo(expected);
            }

        }

        [Fact]
        public async Task GetAll()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var expected = new List<PersonAutoMapperModel>();
                var filterIds = new List<Guid>();
                for (var idx = 0; idx <= 5; idx++)
                {
                    var model = await service.AddAsync(PersonAutoMapperModel.FakerEntity());
                    expected.Add(model);
                    filterIds.Add(model.UniqueId.GetValueOrDefault());
                }

                var records = await service.GetAllAsync<PersonAutoMapperModel>(new OltSearcherGetAll<PersonEntity>());

                records
                    .Where(p => filterIds.Contains(p.UniqueId.GetValueOrDefault()))
                    .OrderBy(p => p.PersonId)
                    .Should()
                    .BeEquivalentTo(expected.OrderBy(p => p.PersonId));
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var expected = new List<PersonAutoMapperModel>();
                var filterIds = new List<Guid>();
                for (var idx = 0; idx <= 5; idx++)
                {
                    var model = service.Add(PersonAutoMapperModel.FakerEntity());
                    expected.Add(model);
                    filterIds.Add(model.UniqueId.GetValueOrDefault());
                }
                service.GetAll<PersonAutoMapperModel>(new OltSearcherGetAll<PersonEntity>())
                    .Where(p => filterIds.Contains(p.UniqueId.GetValueOrDefault()))
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
                var service = provider.GetService<IPersonUniqueIdService>();
                var people = new List<PersonDto>();
                for (var idx = 0; idx <= 115; idx++)
                {
                    people.Add(service.Add<PersonDto, PersonAutoMapperModel>(PersonAutoMapperModel.FakerEntity()));
                }
                var paged = await service.GetPagedAsync<PersonDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams);
                paged.Should().BeEquivalentTo(people.OrderBy(p => p.Last).ThenBy(p => p.First).ThenBy(p => p.PersonId).AsQueryable().ToPaged(pagedParams), opt => opt.WithStrictOrdering());
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();
                var people = new List<PersonDto>();
                for (var idx = 0; idx <= 115; idx++)
                {
                    people.Add(service.Add<PersonDto, PersonAutoMapperModel>(PersonAutoMapperModel.FakerEntity()));
                }
                var paged = service.GetPaged<PersonDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams);
                paged.Should().BeEquivalentTo(people.OrderBy(p => p.Last).ThenBy(p => p.First).ThenBy(p => p.PersonId).AsQueryable().ToPaged(pagedParams), opt => opt.WithStrictOrdering());
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();
                var people = new List<PersonAutoMapperPagedDto>();
                for (var idx = 0; idx <= 115; idx++)
                {
                    people.Add(service.Add<PersonAutoMapperPagedDto, PersonAutoMapperModel>(PersonAutoMapperModel.FakerEntity()));
                } 
                var paged = await service.GetPagedAsync<PersonAutoMapperPagedDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams, orderBy => orderBy.OrderBy(p => p.NameFirst).ThenBy(p => p.NameLast).ThenBy(p => p.Id));
                paged.Should().BeEquivalentTo(people.OrderBy(p => p.First).ThenBy(p => p.Last).ThenBy(p => p.PersonId).AsQueryable().ToPaged(pagedParams), opt => opt.WithStrictOrdering());
                
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();
                var people = new List<PersonAutoMapperPagedDto>();
                for (var idx = 0; idx <= 115; idx++)
                {
                    people.Add(service.Add<PersonAutoMapperPagedDto, PersonAutoMapperModel>(PersonAutoMapperModel.FakerEntity()));
                }
                var paged = service.GetPaged<PersonAutoMapperPagedDto>(new OltSearcherGetAll<PersonEntity>(), pagedParams, orderBy => orderBy.OrderBy(p => p.NameFirst).ThenBy(p => p.NameLast).ThenBy(p => p.Id));
                paged.Should().BeEquivalentTo(people.OrderBy(p => p.First).ThenBy(p => p.Last).ThenBy(p => p.PersonId).AsQueryable().ToPaged(pagedParams), opt => opt.WithStrictOrdering());
            }
        }

        [Fact]
        public async Task GetByIdSearcher()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var newDto = await service.AddAsync(PersonAutoMapperModel.FakerEntity());
                var result = await service.GetAsync<PersonDto>(new OltSearcherGetByUid<PersonEntity>(newDto.UniqueId.GetValueOrDefault()));
                Assert.Equal(newDto.UniqueId, result?.UniqueId);
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var newDto = service.Add(PersonAutoMapperModel.FakerEntity());
                var result = service.Get<PersonDto>(new OltSearcherGetByUid<PersonEntity>(newDto.UniqueId.GetValueOrDefault()));
                Assert.Equal(newDto.UniqueId, result?.UniqueId);
            }


            // ***************** Deleted Records ******************
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var newDto = await service.AddAsync(PersonAutoMapperModel.FakerEntity());
                await service.SoftDeleteAsync(newDto.UniqueId.GetValueOrDefault());
                var result = await service.GetAsync<PersonDto>(new OltSearcherGetByUid<PersonEntity>(newDto.UniqueId.GetValueOrDefault()));
                Assert.Equal(newDto.UniqueId, result?.UniqueId);
            }


            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var newDto = service.Add(PersonAutoMapperModel.FakerEntity());
                service.SoftDelete(newDto.UniqueId.GetValueOrDefault());
                var result = service.Get<PersonDto>(new OltSearcherGetByUid<PersonEntity>(newDto.UniqueId.GetValueOrDefault()));
                Assert.Equal(newDto.UniqueId, result?.UniqueId);
            }


        }

        [Fact]
        public async Task GetByAllSearcher()
        {

            // ***************** Exclude Deleted Records ******************
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var newDto = await service.AddAsync(PersonAutoMapperModel.FakerEntity());
                service.SoftDelete(newDto.UniqueId.GetValueOrDefault());
                var result = service.GetAll<PersonDto>(new OltSearcherGetAll<PersonEntity>()).FirstOrDefault(p => p.UniqueId == newDto.UniqueId);
                Assert.Null(result);
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var newDto = await service.AddAsync(PersonAutoMapperModel.FakerEntity());
                await service.SoftDeleteAsync(newDto.UniqueId.GetValueOrDefault());
                var results = await service.GetAllAsync<PersonDto>(new OltSearcherGetAll<PersonEntity>());
                var result = results.FirstOrDefault(p => p.UniqueId == newDto.UniqueId);
                Assert.Null(result);
            }


            // ***************** Include Deleted Records ******************
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var newDto = await service.AddAsync(PersonAutoMapperModel.FakerEntity());
                await service.SoftDeleteAsync(newDto.UniqueId.GetValueOrDefault());
                var results = await service.GetAllAsync<PersonDto>(new OltSearcherGetAll<PersonEntity>(true));
                var result = results.FirstOrDefault(p => p.UniqueId == newDto.UniqueId);
                Assert.Equal(newDto.UniqueId, result?.UniqueId);
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var newDto = await service.AddAsync(PersonAutoMapperModel.FakerEntity());
                service.SoftDelete(newDto.UniqueId.GetValueOrDefault());
                var result = service.GetAll<PersonDto>(new OltSearcherGetAll<PersonEntity>(true)).FirstOrDefault(p => p.UniqueId == newDto.UniqueId);
                Assert.Equal(newDto.UniqueId, result?.UniqueId);
            }
        }



        [Fact]
        public async Task Count()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var person = await service.AddAsync(PersonAutoMapperModel.FakerEntity());
                Assert.Equal(1, await service.CountAsync(new OltSearcherGetByUid<PersonEntity>(person.UniqueId.Value)));
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var person = await service.AddAsync(PersonAutoMapperModel.FakerEntity());
                Assert.Equal(1, service.Count(new OltSearcherGetByUid<PersonEntity>(person.UniqueId.Value)));
            }
        }



        [Fact]
        public async Task GetByUidSearcher()
        {
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var model = PersonAutoMapperModel.FakerEntity();
                await service.AddAsync(model);
                var result = await service.GetAsync<PersonAutoMapperModel>(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.GetValueOrDefault()));
                result.Should().BeEquivalentTo(model, opt => opt.Excluding(t => t.PersonId));
            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var model = PersonAutoMapperModel.FakerEntity();
                await service.AddAsync(model);
                var result = service.Get<PersonAutoMapperModel>(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.GetValueOrDefault()));
                result.Should().BeEquivalentTo(model, opt => opt.Excluding(t => t.PersonId));
            }


            // Delete
            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();

                var model = PersonAutoMapperModel.FakerEntity();
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
                var service = provider.GetService<IPersonUniqueIdService>();
                var random = Guid.NewGuid();
                var model = await service.AddAsync(PersonDto.FakerEntity());
                Assert.True(await service.AnyAsync(model.UniqueId.Value));
                Assert.True(await service.AnyAsync(p => p.UniqueId == model.UniqueId.Value));

                Assert.False(await service.AnyAsync(random));
                Assert.False(await service.AnyAsync(p => p.UniqueId == random));

                model = service.Add(PersonDto.FakerEntity());
                Assert.True(await service.AnyAsync(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.Value)));
                Assert.False(await service.AnyAsync(new OltSearcherGetByUid<PersonEntity>(Guid.NewGuid())));

            }

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<IPersonUniqueIdService>();
                var random = Guid.NewGuid();
                var model = service.Add(PersonDto.FakerEntity());
                Assert.True(service.Any(model.UniqueId.Value));
                Assert.True(service.Any(p => p.UniqueId == model.UniqueId.Value));

                Assert.False(service.Any(random));
                Assert.False(service.Any(p => p.UniqueId == random));

                model = service.Add(PersonDto.FakerEntity());
                Assert.True(service.Any(new OltSearcherGetByUid<PersonEntity>(model.UniqueId.Value)));
                Assert.False(service.Any(new OltSearcherGetByUid<PersonEntity>(Guid.NewGuid())));

            }
        }

    }
}