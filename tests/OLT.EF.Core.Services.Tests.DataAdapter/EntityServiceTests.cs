using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.Core.Common.Tests.Assets;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Models;
using OLT.EF.Core.Services.Tests.Lib.Abstract;

namespace OLT.EF.Core.Services.Tests.DataAdapter;

public class EntityServiceTests : BaseUnitTests
{

    [Fact]
    public async Task Add()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IUserService>();

            var model = UserModel.FakerEntity();
            (await service.AddAsync(model)).Should().BeEquivalentTo(model, opt => opt.Excluding(t => t.UserId));

            model = UserModel.FakerEntity();
            var result = await service.AddAsync<UserDto, UserModel>(model);
            Assert.Equal(model.UserGuid, result.UserGuid);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IUserService>();

            var model = UserModel.FakerEntity();
            service.Add(model).Should().BeEquivalentTo(model, opt => opt.Excluding(t => t.UserId));
            model = UserModel.FakerEntity();
            var result = service.Add<UserDto, UserModel>(model);
            Assert.Equal(model.UserGuid, result.UserGuid);
        }

    }

    [Fact]
    public async Task AddList()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IUserService>();

            var list = new List<UserModel>
                {
                    UserModel.FakerEntity(),
                    UserModel.FakerEntity(),
                    UserModel.FakerEntity()
                };

            (await service.AddAsync(list)).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.UserId));
            (await service.AddAsync(list.ToArray())).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.UserId));
            (await service.AddAsync(list.AsEnumerable())).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.UserId));

            var dtoList = list.Select(s => new UserDto
            {
                UserGuid = s.UserGuid,
                First = s.Name.First,
                Middle = s.Name.Middle,
                Last = s.Name.Last,
                Suffix = s.Name.Suffix,
            }).ToList();

            (await service.AddAsync<UserDto, UserModel>(list)).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.UserId));
            (await service.AddAsync<UserDto, UserModel>(list.ToArray())).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.UserId));
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IUserService>();

            var list = new List<UserModel>
                {
                    UserModel.FakerEntity(),
                    UserModel.FakerEntity(),
                    UserModel.FakerEntity()
                };

            service.Add(list).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.UserId));
            service.Add(list.ToArray()).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.UserId));
            service.Add(list.AsEnumerable()).Should().BeEquivalentTo(list, opt => opt.Excluding(t => t.UserId));

            var dtoList = list.Select(s => new UserDto
            {
                UserGuid = s.UserGuid,
                First = s.Name.First,
                Middle = s.Name.Middle,
                Last = s.Name.Last,
                Suffix = s.Name.Suffix,
            }).ToList();

            service.Add<UserDto, UserModel>(list).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.UserId));
            service.Add<UserDto, UserModel>(list.ToArray()).Should().BeEquivalentTo(dtoList, opt => opt.Excluding(t => t.UserId));
        }


    }


    [Fact]
    public async Task Get()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IUserService>();

            var model = await service.AddAsync(UserModel.FakerEntity());
            (await service.GetAsync<UserModel>(p => p.Id == model.UserId.Value)).Should().BeEquivalentTo(model);
            (await service.GetAsync<UserModel>(new OltSearcherGetByUid<UserEntity>(model.UserGuid))).Should().BeEquivalentTo(model);
            (await service.GetAsync<UserModel>(false, CancellationToken.None, new OltSearcherGetByUid<UserEntity>(model.UserGuid), new OltSearcherGetById<UserEntity>(model.UserId.Value))).Should().BeEquivalentTo(model);
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IUserService>();

            var model = service.Add(UserModel.FakerEntity());
            service.Get<UserModel>(p => p.Id == model.UserId.Value).Should().BeEquivalentTo(model);
            service.Get<UserModel>(new OltSearcherGetByUid<UserEntity>(model.UserGuid)).Should().BeEquivalentTo(model);
            service.Get<UserModel>(false, new OltSearcherGetByUid<UserEntity>(model.UserGuid), new OltSearcherGetById<UserEntity>(model.UserId.Value)).Should().BeEquivalentTo(model);
        }
    }


    [Fact]
    public async Task GetAll()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IUserService>();
            var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();

            var model = await service.AddAsync(UserModel.FakerEntity());
            (await service.GetAllAsync<UserModel>(p => p.Id == model.UserId.Value)).FirstOrDefault().Should().BeEquivalentTo(model);

            adapterResolver.Map(service.GetRepository().Where(p => p.Id == model.UserId.Value).FirstOrDefault(), new UserModel()).Should().BeEquivalentTo(model);

            (await service.GetAllAsync<UserModel>(new OltSearcherGetByUid<UserEntity>(model.UserGuid))).FirstOrDefault().Should().BeEquivalentTo(model);
            (await service.GetAllAsync<UserModel>(false, CancellationToken.None, new OltSearcherGetByUid<UserEntity>(model.UserGuid), new OltSearcherGetById<UserEntity>(model.UserId.Value))).FirstOrDefault().Should().BeEquivalentTo(model);

            var model2 = await service.AddAsync(UserModel.FakerEntity());
            var compareTo = new List<UserModel>();
            compareTo.Add(model);
            compareTo.Add(model2);

            var compareList = await service.GetAllAsync<UserModel>(new IdListSearcher<UserEntity>(model.UserId.Value, model2.UserId.Value), p => p.OrderBy(t => t.LastName).ThenBy(t => t.FirstName));
            compareList.Should().BeEquivalentTo(compareTo.OrderBy(p => p.Name.Last).ThenBy(p => p.Name.First), options => options.WithStrictOrdering());

            var compareList2 = await service.GetAllAsync<UserModel>(false, p => p.OrderBy(t => t.LastName).ThenBy(t => t.FirstName), CancellationToken.None, new IdListSearcher<UserEntity>(model.UserId.Value, model2.UserId.Value), new OltSearcherGetAll<UserEntity>());
            compareList2.Should().BeEquivalentTo(compareTo.OrderBy(p => p.Name.Last).ThenBy(p => p.Name.First), options => options.WithStrictOrdering());
        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IUserService>();
            var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();

            var model = service.Add(UserModel.FakerEntity());
            service.GetAll<UserModel>(p => p.Id == model.UserId.Value).FirstOrDefault().Should().BeEquivalentTo(model);

            adapterResolver.Map(service.GetRepository().Where(p => p.Id == model.UserId.Value).FirstOrDefault(), new UserModel()).Should().BeEquivalentTo(model);

            service.GetAll<UserModel>(new OltSearcherGetByUid<UserEntity>(model.UserGuid)).FirstOrDefault().Should().BeEquivalentTo(model);
            service.GetAll<UserModel>(false, new OltSearcherGetByUid<UserEntity>(model.UserGuid), new OltSearcherGetById<UserEntity>(model.UserId.Value)).FirstOrDefault().Should().BeEquivalentTo(model);


            var model2 = service.Add(UserModel.FakerEntity());
            var compareTo = new List<UserModel>();
            compareTo.Add(model);
            compareTo.Add(model2);
            var searcher = new IdListSearcher<UserEntity>(model.UserId.Value, model2.UserId.Value);

            var compareList = service.GetAll<UserModel>(searcher, p => p.OrderBy(t => t.LastName).ThenBy(t => t.FirstName));
            compareList.Should().BeEquivalentTo(compareTo.OrderBy(p => p.Name.Last).ThenBy(p => p.Name.First), options => options.WithStrictOrdering());

            var compareList2 = service.GetAll<UserModel>(false, p => p.OrderBy(t => t.LastName).ThenBy(t => t.FirstName), new IdListSearcher<UserEntity>(model.UserId.Value, model2.UserId.Value), new OltSearcherGetAll<UserEntity>());
            compareList2.Should().BeEquivalentTo(compareTo.OrderBy(p => p.Name.Last).ThenBy(p => p.Name.First), options => options.WithStrictOrdering());
        }

    }

    [Fact]
    public async Task GetPaged()
    {
        var pagedParams = new OltPagingParams { Page = 1, Size = 3 };
        var list = new List<UserModel>();

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IUserService>();

            for (var idx = 0; idx <= 117; idx++)
            {
                list.Add(await service.AddAsync(UserModel.FakerEntity()));
            }

            var expected = service.GetAll<UserModel>(new OltSearcherGetAll<UserEntity>())
                .OrderBy(p => p.Name.Last).ThenBy(p => p.Name.First).ThenBy(p => p.UserId)
                .AsQueryable()
                .ToPaged(pagedParams);

            var paged = await service.GetPagedAsync<UserModel>(new OltSearcherGetAll<UserEntity>(), pagedParams);
            paged.Data.Should().BeEquivalentTo(expected.Data, opt => opt.WithStrictOrdering());

            paged = service.GetPaged<UserModel>(new OltSearcherGetAll<UserEntity>(), pagedParams, null);
            paged.Should().BeEquivalentTo(expected);


            paged = service.GetPaged<UserModel>(new OltSearcherGetAll<UserEntity>(), pagedParams, queryable => queryable.OrderBy(p => p.LastName).ThenBy(p => p.Id));
            paged.Should().BeEquivalentTo(list.AsQueryable().ToPaged(pagedParams, order => order.OrderBy(p => p.Name.Last).ThenBy(p => p.UserId)), opt => opt.WithStrictOrdering());


        }


    }


    [Fact]
    public async Task GetSafeTests()
    {
        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IUserService>();

            var model = await service.AddAsync(UserModel.FakerEntity());
            var result = await service.GetSafeTestAsync<UserModel>(new OltSearcherGetByUid<UserEntity>(model.UserGuid));
            result.Should().BeEquivalentTo(model);

            result = await service.GetSafeTestAsync<UserModel>(p => p.UniqueId == model.UserGuid);
            result.Should().BeEquivalentTo(model);

            result = await service.GetSafeTestAsync<UserModel>(false, CancellationToken.None, new OltSearcherGetByUid<UserEntity>(model.UserGuid), new OltSearcherGetById<UserEntity>(model.UserId.Value));
            result.Should().BeEquivalentTo(model);

            Func<Task> action = async () => await service.GetSafeTestAsync<UserModel>(new OltSearcherGetByUid<UserEntity>(Guid.NewGuid()));
            await action.Should().ThrowAsync<OltRecordNotFoundException>();

            action = async () => await service.GetSafeTestAsync<UserModel>(p => p.UniqueId == Guid.NewGuid());
            await action.Should().ThrowAsync<OltRecordNotFoundException>();

            action = async () => await service.GetSafeTestAsync<UserModel>(false, CancellationToken.None, new OltSearcherGetByUid<UserEntity>(Guid.NewGuid()), new OltSearcherGetById<UserEntity>(model.UserId.Value));
            await action.Should().ThrowAsync<OltRecordNotFoundException>();

        }

        using (var provider = BuildProvider())
        {
            var service = provider.GetRequiredService<IUserService>();

            var model = service.Add(UserModel.FakerEntity());
            var result = service.GetSafeTest<UserModel>(new OltSearcherGetByUid<UserEntity>(model.UserGuid));
            result.Should().BeEquivalentTo(model);

            result = service.GetSafeTest<UserModel>(p => p.UniqueId == model.UserGuid);
            result.Should().BeEquivalentTo(model);

            result = service.GetSafeTest<UserModel>(false, new OltSearcherGetByUid<UserEntity>(model.UserGuid), new OltSearcherGetById<UserEntity>(model.UserId.Value));
            result.Should().BeEquivalentTo(model);

            Action action = () => service.GetSafeTest<UserModel>(new OltSearcherGetByUid<UserEntity>(Guid.NewGuid()));
            action.Should().Throw<OltRecordNotFoundException>();

            action = () => service.GetSafeTest<UserModel>(p => p.UniqueId == Guid.NewGuid());
            action.Should().Throw<OltRecordNotFoundException>();

            action = () => service.GetSafeTest<UserModel>(false, new OltSearcherGetByUid<UserEntity>(Guid.NewGuid()), new OltSearcherGetById<UserEntity>(model.UserId.Value));
            action.Should().Throw<OltRecordNotFoundException>();
        }
    }

}
