using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Core.Rules.Extensions
{
    public static class OltActionRuleExtensions
    {
        /// <summary>
        /// Rule Execute Extensions
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="rule"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task ExecuteAsync<TContext>(this IOltActionRule rule, TContext context)
            where TContext : DbContext, IOltDbContext
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    await rule.ExecuteAsync(transaction);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
