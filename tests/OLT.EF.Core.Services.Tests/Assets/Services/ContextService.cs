using Microsoft.EntityFrameworkCore;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{
    public interface IContextService : IOltCoreService
    {
        PersonEntity CreatePerson();
        UserEntity CreateUser();

        PersonEntity Get(bool includeDeleted, params IOltSearcher<PersonEntity>[] searchers);
        PersonEntity Get(IOltSearcher<PersonEntity> searcher);
        Task<PersonEntity> GetAsync(IOltSearcher<PersonEntity> searcher);
        List<PersonEntity> Get(bool includeDeleted);
        List<PersonEntity> GetPeopleOrdered(bool includeDeleted);
        List<PersonEntity> GetPeopleOrdered(IOltSearcher<PersonEntity> searcher);
        List<PersonEntity> GetPeopleOrdered(params IOltSearcher<PersonEntity>[] searchers);
        List<PersonEntity> GetNonDeleted();

        List<PersonAutoMapperModel> GetAllPeople();
        List<PersonAutoMapperModel> GetAllPeopleSearcher();
        List<PersonAutoMapperModel> GetAllPeopleOrdered();
        Task<List<PersonAutoMapperModel>> GetAllPeopleAsync();
        Task<List<PersonAutoMapperModel>> GetAllPeopleSearcherAsync();

        List<UserModel> GetAllUsers();
        List<UserModel> GetAllUsersSearcher();
        Task<List<UserModel>> GetAllUsersAsync();
        Task<List<UserModel>> GetAllUsersOrderedAsync();
        Task<List<UserModel>> GetAllUsersSearcherAsync();

        UserDto GetDtoUser(int id);
        Task<UserDto> GetDtoUserAsync(int id);

        List<UserDto> GetAllDtoUsers();
        List<UserDto> GetAllDtoUsersSearcher();
        Task<List<UserDto>> GetAllDtoUsersAsync();
        Task<List<UserDto>> GetAllDtoUsersSearcherAsync();


        bool Delete<TEntity>(int id) where TEntity : class, IOltEntityId;
        Task<bool> DeleteAsync<TEntity>(int id) where TEntity : class, IOltEntityId;
    }

    public class ContextService : OltContextService<UnitTestContext>, IContextService
    {
        public ContextService(IOltServiceManager serviceManager, UnitTestContext context) : base(serviceManager, context)
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

        public PersonEntity Get(bool includeDeleted, params IOltSearcher<PersonEntity>[] searchers) => GetQueryable(includeDeleted, searchers).FirstOrDefault();
        public PersonEntity Get(IOltSearcher<PersonEntity> searcher) => GetQueryable(searcher).FirstOrDefault();
        public List<PersonEntity> Get(bool includeDeleted) => GetQueryable<PersonEntity>(includeDeleted).ToList();
        public List<PersonEntity> GetNonDeleted() => InitializeQueryable<PersonEntity>().ToList();
        public async Task<PersonEntity> GetAsync(IOltSearcher<PersonEntity> searcher) => await GetQueryable(searcher).FirstOrDefaultAsync();

        public List<PersonEntity> GetPeopleOrdered(bool includeDeleted) => GetQueryable<PersonEntity>(includeDeleted, p => p.OrderBy(t => t.NameLast).ThenBy(t => t.NameFirst)).ToList();
        public List<PersonEntity> GetPeopleOrdered(IOltSearcher<PersonEntity> searcher) => GetQueryable(searcher, p => p.OrderBy(t => t.NameLast).ThenBy(t => t.NameFirst)).ToList();
        public List<PersonEntity> GetPeopleOrdered(params IOltSearcher<PersonEntity>[] searchers) => GetQueryable(false, p => p.OrderBy(t => t.NameLast).ThenBy(t => t.NameFirst), searchers).ToList();


        public List<PersonAutoMapperModel> GetAllPeople() => GetAll<PersonEntity, PersonAutoMapperModel>(Context.People).ToList();
        public List<PersonAutoMapperModel> GetAllPeopleSearcher() => GetAll<PersonEntity, PersonAutoMapperModel>(new OltSearcherGetAll<PersonEntity>()).ToList();
        public List<PersonAutoMapperModel> GetAllPeopleOrdered() => GetAll<PersonEntity, PersonAutoMapperModel>(new OltSearcherGetAll<PersonEntity>(), p => p.OrderBy(t => t.NameLast).ThenBy(t => t.NameFirst)).ToList();
        public async Task<List<PersonAutoMapperModel>> GetAllPeopleAsync() => (await GetAllAsync<PersonEntity, PersonAutoMapperModel>(Context.People)).ToList();
        public async Task<List<PersonAutoMapperModel>> GetAllPeopleSearcherAsync() => (await GetAllAsync<PersonEntity, PersonAutoMapperModel>(new OltSearcherGetAll<PersonEntity>())).ToList();

        public List<UserModel> GetAllUsers() => GetAll<UserEntity, UserModel>(new OltSearcherGetAll<UserEntity>()).ToList();
        public List<UserModel> GetAllUsersSearcher() => GetAll<UserEntity, UserModel>(new OltSearcherGetAll<UserEntity>()).ToList();
        public async Task<List<UserModel>> GetAllUsersOrderedAsync() => (await GetAllAsync<UserEntity, UserModel>(new OltSearcherGetAll<UserEntity>(), p => p.OrderBy(t => t.LastName).ThenBy(t => t.FirstName))).ToList();
        public async Task<List<UserModel>> GetAllUsersAsync() => (await GetAllAsync<UserEntity, UserModel>(Context.Users)).ToList();
        public async Task<List<UserModel>> GetAllUsersSearcherAsync() => (await GetAllAsync<UserEntity, UserModel>(new OltSearcherGetAll<UserEntity>())).ToList();
        public UserDto GetDtoUser(int id) => Get<UserEntity, UserDto>(Context.Users.Where(p => p.Id == id));
        public async Task<UserDto> GetDtoUserAsync(int id) => await GetAsync<UserEntity, UserDto>(Context.Users.Where(p => p.Id == id));

        public List<UserDto> GetAllDtoUsers() => GetAll<UserEntity, UserDto>(new OltSearcherGetAll<UserEntity>()).ToList();
        public List<UserDto> GetAllDtoUsersSearcher() => GetAll<UserEntity, UserDto>(new OltSearcherGetAll<UserEntity>()).ToList();
        public async Task<List<UserDto>> GetAllDtoUsersAsync() => (await GetAllAsync<UserEntity, UserDto>(Context.Users)).ToList();
        public async Task<List<UserDto>> GetAllDtoUsersSearcherAsync() => (await GetAllAsync<UserEntity, UserDto>(new OltSearcherGetAll<UserEntity>())).ToList();

        public bool Delete<TEntity>(int id) where TEntity : class, IOltEntityId
        {
            var entity = GetQueryable(new OltSearcherGetById<TEntity>(id)).FirstOrDefault();
            return MarkDeleted(entity);
        }

        public async Task<bool> DeleteAsync<TEntity>(int id) where TEntity : class, IOltEntityId
        {
            var entity = await GetQueryable(new OltSearcherGetById<TEntity>(id)).FirstOrDefaultAsync();
            return await MarkDeletedAsync(entity);
        }
    }
}
