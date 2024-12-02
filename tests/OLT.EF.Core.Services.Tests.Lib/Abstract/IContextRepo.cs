using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Models;

namespace OLT.EF.Core.Services.Tests.Lib.Abstract;

public interface IContextRepo : IOltCoreService
{
    PersonEntity CreatePerson();
    UserEntity CreateUser();

    PersonEntity? Get(bool includeDeleted, params IOltSearcher<PersonEntity>[] searchers);
    PersonEntity? Get(IOltSearcher<PersonEntity> searcher);
    Task<PersonEntity?> GetAsync(IOltSearcher<PersonEntity> searcher);
    List<PersonEntity> Get(bool includeDeleted);
    List<PersonEntity> GetPeopleOrdered(bool includeDeleted);
    List<PersonEntity> GetPeopleOrdered(IOltSearcher<PersonEntity> searcher);
    List<PersonEntity> GetPeopleOrdered(params IOltSearcher<PersonEntity>[] searchers);
    List<PersonEntity> GetNonDeleted();

    List<PersonModel> GetAllPeople();
    List<PersonModel> GetAllPeopleSearcher();
    List<PersonModel> GetAllPeopleOrdered();
    Task<List<PersonModel>> GetAllPeopleAsync();
    Task<List<PersonModel>> GetAllPeopleSearcherAsync();

    List<UserModel> GetAllUsers();
    List<UserModel> GetAllUsersSearcher();
    Task<List<UserModel>> GetAllUsersAsync();
    Task<List<UserModel>> GetAllUsersOrderedAsync();
    Task<List<UserModel>> GetAllUsersSearcherAsync();

    UserDto? GetDtoUser(int id);
    UserDto? GetDtoUser(IOltSearcher<UserEntity> searcher);
    Task<UserDto?> GetDtoUserAsync(IOltSearcher<UserEntity> searcher);
    Task<UserDto?> GetDtoUserAsync(int id);

    List<UserDto> GetAllDtoUsers();
    List<UserDto> GetAllDtoUsersSearcher();
    Task<List<UserDto>> GetAllDtoUsersAsync();
    Task<List<UserDto>> GetAllDtoUsersSearcherAsync();


    bool Delete<TEntity>(int id) where TEntity : class, IOltEntityId;
    Task<bool> DeleteAsync<TEntity>(int id) where TEntity : class, IOltEntityId;


    Task DbTransactionAsync(bool throwError);
    Task<TModel> DbTransactionAsync<TModel>(bool throwError) where TModel : class, new();
}
