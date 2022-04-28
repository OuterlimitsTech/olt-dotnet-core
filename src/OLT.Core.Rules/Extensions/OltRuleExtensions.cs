using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace OLT.Core
{
    public static class OltRuleExtensions
    {

        public static async Task<IOltRuleResult> ExecuteRuleAsync(this DbContext context, IOltRuleBuilder rule)
        {
            IOltRuleResult result;

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    result = await rule.ExecuteAsync(transaction);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }           

            return result;
        }
    }
}
