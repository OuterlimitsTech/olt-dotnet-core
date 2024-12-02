using Microsoft.EntityFrameworkCore;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Models;
using OLT.EF.Core.Services.Tests.Lib.Abstract;

namespace OLT.EF.Core.Services.Tests.Lib.Repos;

public class ContextRepo : OltContextService<TestDbContext>, IContextRepo
{
    public ContextRepo(IOltServiceManager serviceManager, TestDbContext context) : base(serviceManager, context)
    {
    }

    public PersonEntity CreatePerson()
    {
        var entity = PersonEntity.FakerEntity();
        Context.Add(entity);
        SaveChanges();
        return entity;
    }

    public UserEntity CreateUser()
    {
        var entity = UserEntity.FakerEntity();
        Context.Add(entity);
        SaveChanges();
        return entity;
    }

    public PersonEntity? Get(bool includeDeleted, params IOltSearcher<PersonEntity>[] searchers) => GetQueryable(includeDeleted, searchers).FirstOrDefault();
    public PersonEntity? Get(IOltSearcher<PersonEntity> searcher) => GetQueryable(searcher).FirstOrDefault();
    public List<PersonEntity> Get(bool includeDeleted) => GetQueryable<PersonEntity>(includeDeleted).ToList();
    public List<PersonEntity> GetNonDeleted() => InitializeQueryable<PersonEntity>(false).ToList();
    public async Task<PersonEntity?> GetAsync(IOltSearcher<PersonEntity> searcher) => await GetQueryable(searcher).FirstOrDefaultAsync();

    public List<PersonEntity> GetPeopleOrdered(bool includeDeleted) => GetQueryable<PersonEntity>(includeDeleted, p => p.OrderBy(t => t.NameLast).ThenBy(t => t.NameFirst)).ToList();
    public List<PersonEntity> GetPeopleOrdered(IOltSearcher<PersonEntity> searcher) => GetQueryable(searcher, p => p.OrderBy(t => t.NameLast).ThenBy(t => t.NameFirst)).ToList();
    public List<PersonEntity> GetPeopleOrdered(params IOltSearcher<PersonEntity>[] searchers) => GetQueryable(false, p => p.OrderBy(t => t.NameLast).ThenBy(t => t.NameFirst), searchers).ToList();


    public List<PersonModel> GetAllPeople() => GetAll<PersonEntity, PersonModel>(Context.People).ToList();
    public List<PersonModel> GetAllPeopleSearcher() => GetAll<PersonEntity, PersonModel>(new OltSearcherGetAll<PersonEntity>()).ToList();
    public List<PersonModel> GetAllPeopleOrdered() => GetAll<PersonEntity, PersonModel>(new OltSearcherGetAll<PersonEntity>(), p => p.OrderBy(t => t.NameLast).ThenBy(t => t.NameFirst)).ToList();
    public async Task<List<PersonModel>> GetAllPeopleAsync() => (await GetAllAsync<PersonEntity, PersonModel>(Context.People)).ToList();
    public async Task<List<PersonModel>> GetAllPeopleSearcherAsync() => (await GetAllAsync<PersonEntity, PersonModel>(new OltSearcherGetAll<PersonEntity>())).ToList();

    public List<UserModel> GetAllUsers() => GetAll<UserEntity, UserModel>(new OltSearcherGetAll<UserEntity>()).ToList();
    public List<UserModel> GetAllUsersSearcher() => GetAll<UserEntity, UserModel>(new OltSearcherGetAll<UserEntity>()).ToList();
    public async Task<List<UserModel>> GetAllUsersOrderedAsync() => (await GetAllAsync<UserEntity, UserModel>(new OltSearcherGetAll<UserEntity>(), p => p.OrderBy(t => t.LastName).ThenBy(t => t.FirstName))).ToList();
    public async Task<List<UserModel>> GetAllUsersAsync() => (await GetAllAsync<UserEntity, UserModel>(Context.Users)).ToList();
    public async Task<List<UserModel>> GetAllUsersSearcherAsync() => (await GetAllAsync<UserEntity, UserModel>(new OltSearcherGetAll<UserEntity>())).ToList();


    public UserDto? GetDtoUser(IOltSearcher<UserEntity> searcher) => Get<UserEntity, UserDto>(searcher);
    public UserDto? GetDtoUser(int id) => Get<UserEntity, UserDto>(Context.Users.Where(p => p.Id == id));

    public async Task<UserDto?> GetDtoUserAsync(IOltSearcher<UserEntity> searcher) => await GetAsync<UserEntity, UserDto>(searcher);
    public async Task<UserDto?> GetDtoUserAsync(int id) => await GetAsync<UserEntity, UserDto>(Context.Users.Where(p => p.Id == id));

    public List<UserDto> GetAllDtoUsers() => GetAll<UserEntity, UserDto>(new OltSearcherGetAll<UserEntity>()).ToList();
    public List<UserDto> GetAllDtoUsersSearcher() => GetAll<UserEntity, UserDto>(new OltSearcherGetAll<UserEntity>()).ToList();
    public async Task<List<UserDto>> GetAllDtoUsersAsync() => (await GetAllAsync<UserEntity, UserDto>(Context.Users)).ToList();
    public async Task<List<UserDto>> GetAllDtoUsersSearcherAsync() => (await GetAllAsync<UserEntity, UserDto>(new OltSearcherGetAll<UserEntity>())).ToList();

    public bool Delete<TEntity>(int id) where TEntity : class, IOltEntityId
    {
        var entity = GetQueryable(new OltSearcherGetById<TEntity>(id)).FirstOrDefault() ?? throw new Exception("Record Not Found");
        return MarkDeleted(entity);
    }

    public async Task<bool> DeleteAsync<TEntity>(int id) where TEntity : class, IOltEntityId
    {
        var entity = await GetQueryable(new OltSearcherGetById<TEntity>(id)).FirstOrDefaultAsync() ?? throw new Exception("Record Not Found");
        return await MarkDeletedAsync(entity);
    }

    public async Task DbTransactionAsync(bool throwError)
    {
        await WithDbTransactionAsync(() =>
        {
            if (throwError)
            {
                throw new Exception("TestError");
            }

            return Task.CompletedTask;
        });
    }

    public async Task<TModel> DbTransactionAsync<TModel>(bool throwError) where TModel : class, new()
    {
        return await WithDbTransactionAsync<TModel>(() =>
        {
            if (throwError)
            {
                throw new Exception("TestError");
            }

            return Task.FromResult(new TModel());
        });
    }

}
