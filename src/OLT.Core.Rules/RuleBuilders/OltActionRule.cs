using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OLT.Core
{
    public abstract class OltActionRule<T> : OltRule, IOltActionRule
        where T : OltActionRule<T>
    {
        protected virtual string SavePointName => this.GetType().Name.Left(32); //Max Length of DB Transaction SavePoint is 32 characters
        private readonly Dictionary<string, object> _params = new Dictionary<string, object>();
        private readonly Dictionary<string, IOltCoreService> _services = new Dictionary<string, IOltCoreService>();
        protected Dictionary<string, OltDependentRule> DependentRules = new Dictionary<string, OltDependentRule>();

        protected virtual bool HasTransaction { get; set; }
        protected abstract Task RunRuleAsync();

        #region [ Execute ]

        public virtual Task<List<OltRuleCanRunException>> CanExecuteAsync()
        {
            var list = new List<OltRuleCanRunException>();
            if (!HasTransaction)
            {
                list.Add(new OltRuleMissingTransactionException(this));
            }
            return Task.FromResult(list);
        }

        protected virtual async Task RunDependentRulesAsync(OltDependentRuleRunTypes type, IDbContextTransaction dbTransaction)
        {
            var rules = DependentRules.Where(p => p.Value.Type == type).Select(s => s.Value);
            foreach (var value in rules)
            {
                await value.Rule.ExecuteAsync(dbTransaction);
            }
        }

        /// <summary>
        /// Run Rule using new Transaction
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task ExecuteAsync<TContext>(TContext context)
            where TContext : DbContext, IOltDbContext
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    await ExecuteAsync(transaction);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        /// <summary>
        /// Run Rule with DB Transaction Savepoint
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        /// <exception cref="OltRuleException"></exception>        
        /// <exception cref="OltRuleCanRunException"></exception>
        public virtual async Task ExecuteAsync(IDbContextTransaction dbTransaction)
        {
            HasTransaction = dbTransaction != null;

            var errors = await CanExecuteAsync();
            if (errors.Any())
            {
                throw new AggregateException(errors);
            }

            try
            {

                await dbTransaction.CreateSavepointAsync(this.SavePointName);

                await RunDependentRulesAsync(OltDependentRuleRunTypes.RunBefore, dbTransaction);
                await RunRuleAsync();
                await RunDependentRulesAsync(OltDependentRuleRunTypes.RunAfter, dbTransaction);

                await dbTransaction.ReleaseSavepointAsync(this.SavePointName);

            }
            catch (Exception)
            {
                await dbTransaction.RollbackToSavepointAsync(this.SavePointName);
                throw;
            }
        }


        #endregion

        #region [ Parameter ]

        public T WithParameter<TData>(TData data) where TData : class
        {
            var fullName = data.GetType().FullName;
            if (!_params.ContainsKey(fullName))
            {
                _params.Add(fullName, data);
            }
            return (T)this;
        }

        /// <summary>
        /// Gets a a registered class by type.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="throwMissingException"></param>
        /// <returns></returns>
        /// <exception cref="OltRuleMissingParameterException{TParameter}"></exception>
        protected TParameter GetParameter<TParameter>(bool throwMissingException = true) where TParameter : class
        {
            var fullName = typeof(TParameter).FullName;
            var result = _params.FirstOrDefault(p => p.Key == fullName);
            if (result.Value == null)
            {
                if (throwMissingException)
                {
                    throw new OltRuleMissingParameterException<TParameter>(this);
                }
                return null;
            }
            return result.Value as TParameter;
        }

        #endregion

        #region [ Dependent Rule ]

        /// <summary>
        /// Run Additional Rules (ordered by add)
        /// </summary>
        /// <typeparam name="TRule"></typeparam>
        /// <param name="rule"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual T WithDependentRule<TRule>(TRule rule, OltDependentRuleRunTypes type) where TRule : class, IOltActionRule
        {
            if (!DependentRules.ContainsKey(rule.RuleName))
            {
                DependentRules.Add(rule.RuleName, new OltDependentRule(type, rule));
            }
            return (T)this;
        }

        #endregion

        #region [ Service ]

        public T WithService<TService>(TService service) where TService : class, IOltCoreService
        {
            var fullName = service.GetType().FullName;
            if (!_services.ContainsKey(fullName))
            {
                _services.Add(fullName, service);
            }
            return (T)this;
        }

        /// <summary>
        /// Gets registred <seealso cref="IOltCoreService"/> 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="throwMissingException"></param>
        /// <returns></returns>
        /// <exception cref="OltRuleMissingServiceException{TService}"></exception>
        protected TService GetService<TService>(bool throwMissingException = true) where TService : class, IOltCoreService
        {
            var result = _services.FirstOrDefault(p => p.Value.GetType().Implements<TService>());
            if (result.Value == null)
            {
                if (throwMissingException)
                {
                    throw new OltRuleMissingServiceException<TService>(this);
                }
                return null;
            }
            return result.Value as TService;
        }

        #endregion

    }
}
