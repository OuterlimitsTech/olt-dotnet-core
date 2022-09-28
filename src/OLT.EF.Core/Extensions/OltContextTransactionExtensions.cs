using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using System;

namespace OLT.Core
{
    public static class OltContextTransactionExtensions
    {
        public static async Task<TResult> CreateSubTransactionAsync<TResult>(this IDbContextTransaction dbTransaction, Func<Task<TResult>> action)
        {
            var savePointName = Guid.NewGuid().ToString().Left(32);
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
            var savePointName = Guid.NewGuid().ToString().Left(32);
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