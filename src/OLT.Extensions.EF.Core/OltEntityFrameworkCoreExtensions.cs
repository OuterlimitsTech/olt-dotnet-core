using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using System;

namespace OLT.Core
{
    public static class OltEntityFrameworkCoreExtensions
    {
        public static async Task<TResult> UsingDbTransactionAsync<TResult>(this DatabaseFacade database, Func<Task<TResult>> action)
        {
            if (database.CurrentTransaction == null)
            {
                using var transaction = await database.BeginTransactionAsync();
                try
                {
                    var result = await CreateSubTransactionAsync(transaction, action);
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                return await CreateSubTransactionAsync(database.CurrentTransaction, action);
            }
        }

        public static async Task UsingDbTransactionAsync(this DatabaseFacade database, Func<Task> action)
        {
            if (database.CurrentTransaction == null)
            {
                using var transaction = await database.BeginTransactionAsync();
                try
                {
                    await CreateSubTransactionAsync(transaction, action);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                await CreateSubTransactionAsync(database.CurrentTransaction, action);
            }
        }

        public static async Task<TResult> CreateSubTransactionAsync<TResult>(this IDbContextTransaction dbTransaction, Func<Task<TResult>> action)
        {
            var savePointName = Guid.NewGuid().ToString("N").Substring(0, 32);
            try
            {
                await dbTransaction.CreateSavepointAsync(savePointName);
                var result = await action();
                await dbTransaction.ReleaseSavepointAsync(savePointName);
                return result;
            }
            catch (Exception)
            {
                await dbTransaction.RollbackToSavepointAsync(savePointName);
                throw;
            }
        }

        public static async Task CreateSubTransactionAsync(this IDbContextTransaction dbTransaction, Func<Task> action)
        {
            var savePointName = Guid.NewGuid().ToString("N").Substring(0, 32);
            try
            {
                await dbTransaction.CreateSavepointAsync(savePointName);
                await action();
                await dbTransaction.ReleaseSavepointAsync(savePointName);
            }
            catch (Exception)
            {
                await dbTransaction.RollbackToSavepointAsync(savePointName);
                throw;
            }
        }
    }
}