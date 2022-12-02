using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OLT.Core
{
    public abstract class OltCommandBus<TContext> : OltDisposable, IOltCommandBus
        where TContext : DbContext
    {
        public OltCommandBus(
            IOltIdentity identity,
            IEnumerable<IOltCommandHandler> handlers,
            TContext context)
        {
            Context = context;
            Identity = identity;
            Handlers = handlers.ToList();
        }

        protected virtual TContext Context { get; }
        protected virtual IOltIdentity Identity { get; }
        protected virtual List<IOltCommandHandler> Handlers { get; }

        /// <summary>
        /// Attempts to locate <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        protected virtual IOltCommandHandler GetHandler(IOltCommand command)
        {
            var handler = Handlers.SingleOrDefault(p => p.ActionName == command.ActionName);
            if (handler == null)
            {
                throw new OltCommandHandlerNotFoundException(command);
            }
            return handler;
        }

        protected virtual async Task<IOltCommandValidationResult> ValidateAsync(IOltCommand command)
        {
            var handler = GetHandler(command);
            return await handler.ValidateAsync(Identity, command);
        }

        protected virtual async Task<IOltCommandResult> ExecuteAsync(IOltCommand command)
        {
            var validationResult = await ValidateAsync(command);
            if (!validationResult.Valid)
            {
                throw validationResult.ToException();
            }

            var handler = GetHandler(command);
            return await Context.Database.UsingDbTransactionAsync(async () =>
            {
                return await handler.ExecuteAsync(Identity, command);
            });
        }

        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        public virtual async Task ProcessAsync(IOltCommand command)
        {
            await ExecuteAsync(command);
        }

        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="NullReferenceException">Thrown is command result is null</exception>
        /// <exception cref="InvalidCastException">Thrown if command result can not be cast to <typeparamref name="T"/></exception>
        public virtual async Task<T> ProcessAsync<T>(IOltCommand command)
        {
            var result = await ExecuteAsync(command);
            return result.GetResult<T>();
        }
    }

    public abstract class OltCommandBus<TContext, TIdentity> : OltCommandBus<TContext>
        where TContext : DbContext
        where TIdentity : IOltIdentity
    {
        protected OltCommandBus(
            IOltIdentity identity, 
            IEnumerable<IOltCommandHandler> handlers, 
            TContext context) : base(identity, handlers, context)
        {
            Identity = (TIdentity)identity;
        }

        protected new TIdentity Identity { get; }
    }
}