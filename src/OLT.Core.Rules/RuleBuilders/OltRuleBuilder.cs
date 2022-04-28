using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OLT.Core
{
    public abstract class OltRuleBuilder : OltRule, IOltRuleBuilder
    {
        protected string SavePointName => this.GetType().Name.Left(32); //Max Length of DB Transaction SavePoint is 32 characters

        public virtual List<OltRuleCanRunException> CanExecute()
        {
            var list = new List<OltRuleCanRunException>();
            if (RequiresDbTransaction && !HasTransaction)
            {
                list.Add(new OltRuleMissingTransactionException(this));
            }
            return list;
        }

        protected virtual bool HasTransaction { get; set; }
        public abstract bool RequiresDbTransaction { get; }

        protected abstract Task<IOltRuleResult> RunRuleAsync();

        /// <summary>
        /// Executes Rule
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public async Task<IOltRuleResult> ExecuteAsync()
        {
            var errors = CanExecute();
            if (errors.Any())
            {
                throw new AggregateException(errors);
            }
            return await RunRuleAsync();
        }

        /// <summary>
        /// Run Rule with DB Transaction Savepoint
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public virtual async Task<IOltRuleResult> ExecuteAsync(IDbContextTransaction dbTransaction)
        {
            HasTransaction = dbTransaction != null;

            try
            {
                await dbTransaction.CreateSavepointAsync(this.SavePointName);
                var result = await ExecuteAsync();
                await dbTransaction.ReleaseSavepointAsync(this.SavePointName);
                return result;
            }
            catch (Exception)
            {
                await dbTransaction.RollbackToSavepointAsync(this.SavePointName);
                throw;
            }
        }
    }

    public abstract class OltRuleBuilder<T> : OltRuleBuilderWithService<T>  
        where T : OltRuleBuilder<T>
    {
        private readonly Dictionary<string, IOltRule> _services = new Dictionary<string, IOltRule>();

        public T RunRule<TRule>(TRule rule) where TRule : class, IOltRule
        {
            if (!_services.ContainsKey(rule.RuleName))
            {
                _services.Add(rule.RuleName, rule);
            }
            return (T)this;
        }
    }
}
