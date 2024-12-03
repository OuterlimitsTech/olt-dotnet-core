using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace OLT.Core
{
    public static class OltEntityTransactionExtensions
    {
        public static async Task<TResult> UsingDbTransactionAsync<TResult>(this DatabaseFacade database, Func<Task<TResult>> action, CancellationToken cancellationToken = default)
        {
            if (database.CurrentTransaction == null)
            {
                using var transaction = await database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var result = await CreateSubTransactionAsync(transaction, action);
                    await transaction.CommitAsync(cancellationToken);
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
            else
            {
                return await CreateSubTransactionAsync(database.CurrentTransaction, action, cancellationToken);
            }
        }

        public static async Task UsingDbTransactionAsync(this DatabaseFacade database, Func<Task> action, CancellationToken cancellationToken = default)
        {
            if (database.CurrentTransaction == null)
            {
                using var transaction = await database.BeginTransactionAsync(cancellationToken);
                try
                {
                    await CreateSubTransactionAsync(transaction, action);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
            else
            {
                await CreateSubTransactionAsync(database.CurrentTransaction, action, cancellationToken);
            }
        }

        public static async Task<TResult> CreateSubTransactionAsync<TResult>(this IDbContextTransaction dbTransaction, Func<Task<TResult>> action, CancellationToken cancellationToken = default)
        {
            var savePointName = Guid.NewGuid().ToString("N").Substring(0, 32);
            try
            {
                await dbTransaction.CreateSavepointAsync(savePointName, cancellationToken);
                var result = await action();
                await dbTransaction.ReleaseSavepointAsync(savePointName, cancellationToken);
                return result;
            }
            catch (Exception)
            {
                await dbTransaction.RollbackToSavepointAsync(savePointName, cancellationToken);
                throw;
            }
        }

        public static async Task CreateSubTransactionAsync(this IDbContextTransaction dbTransaction, Func<Task> action, CancellationToken cancellationToken = default)
        {
            var savePointName = Guid.NewGuid().ToString("N").Substring(0, 32);
            try
            {
                await dbTransaction.CreateSavepointAsync(savePointName, cancellationToken);
                await action();
                await dbTransaction.ReleaseSavepointAsync(savePointName, cancellationToken);
            }
            catch (Exception)
            {
                await dbTransaction.RollbackToSavepointAsync(savePointName, cancellationToken);
                throw;
            }
        }
    }

}