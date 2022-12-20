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
        protected OltCommandBus(
            IEnumerable<IOltCommandHandler> handlers,
            TContext context)
        {
            Context = context;
            Handlers = handlers.ToList();
        }

        protected virtual TContext Context { get; }
        protected virtual List<IOltCommandHandler> Handlers { get; }
                
        /// <summary>
        /// Attempts to locate <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="OltCommandHandlerMultipleException"></exception>
        protected virtual IOltCommandHandler GetHandler(IOltCommand command)
        {
            var handlerCount = Handlers.Count(p => p.ActionName == command.ActionName);
            if (handlerCount == 1)
            {
                return Handlers.Single(p => p.ActionName == command.ActionName);
            }

            if (handlerCount > 1)
            {
                throw new OltCommandHandlerMultipleException(command);
            }

            throw new OltCommandHandlerNotFoundException(command);
        }

        public virtual async Task<IOltCommandValidationResult> ValidateAsync(IOltCommand command)
        {
            var handler = GetHandler(command);
            return await handler.ValidateAsync(this, command);
        }


        /// <summary>
        /// Executes Command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected virtual async Task<IOltCommandBusResult> ExecuteAsync(IOltCommand command)
        {
            var validationResult = await ValidateAsync(command);
            if (!validationResult.Valid)
            {
                throw validationResult.ToException();
            }
            
            var handler = GetHandler(command);

            var result = await Context.Database.UsingDbTransactionAsync(async () =>
            {
                return await handler.ExecuteAsync(this, command);
            });
            
            return OltCommandBusResult.FromCommand(command, result);

        }


        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="OltCommandHandlerMultipleException"></exception>
        public virtual Task ProcessAsync(IOltCommand command)
        {
            return ExecuteAsync(command);
        }

        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="OltCommandHandlerMultipleException"></exception>
        /// <exception cref="OltCommandResultNullException">Thrown is command result is null</exception>
        /// <exception cref="InvalidCastException">Thrown if command result can not be cast to <typeparamref name="T"/></exception>
        public virtual async Task<T> ProcessAsync<T>(IOltCommand command)
        {
            var result = await ExecuteAsync(command);
            return result.CommandResult.GetResult<T>();
        }
    }
}