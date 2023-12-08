using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
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

        protected virtual ConcurrentQueue<IOltAfterCommandQueueItem> PostProcessItems { get; } = new ConcurrentQueue<IOltAfterCommandQueueItem>();

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


        /// <summary>
        /// Validates Command and CommandHandler can Execute
        /// </summary>
        /// <param name="command"></param>
        /// <param name="handler"></param>
        /// <returns></returns>        
        protected virtual Task<IOltCommandValidationResult> ValidateAsync(IOltCommandHandler handler, IOltCommand command)
        {
            return handler.ValidateAsync(this, command);
        }

        /// <summary>
        /// Validates Command and CommandHandler can Execute
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="OltCommandHandlerMultipleException"></exception>
        public virtual Task<IOltCommandValidationResult> ValidateAsync(IOltCommand command)
        {
            return this.ValidateAsync(GetHandler(command), command);
        }

        /// <summary>
        /// Executes Command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        protected virtual async Task<IOltCommandBusResult> ExecuteAsync(IOltCommandHandler handler, IOltCommand command)
        {
            var validationResult = await ValidateAsync(handler, command);
            if (!validationResult.Valid)
            {
                throw validationResult.ToException();
            }

            var result = await Context.Database.UsingDbTransactionAsync(async () =>
            {
                return await handler.ExecuteAsync(this, command);
            });

            await PostExecuteAsync(handler, command, result);

            return OltCommandBusResult.FromCommand(command, result);
        }


        /////// <summary>
        /////// Executes Command
        /////// </summary>
        /////// <param name="command"></param>
        /////// <returns></returns>
        ////protected virtual Task<IOltCommandBusResult> ExecuteAsync(IOltCommand command)
        ////{
        ////    return ExecuteAsync(GetHandler(command), command);
        ////}      

        /// <summary>
        /// Runs in order (or Queues if in Transaction) the <seealso cref="IOltPostCommandHandler.PostExecuteAsync(IOltCommand, IOltCommandResult)"/> 
        /// </summary>
        /// <param name="currentHandler"></param>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual async Task PostExecuteAsync<TResult>(IOltCommandHandler currentHandler, IOltCommand command, TResult result)
            where TResult: notnull
        {

            if (currentHandler is IOltPostCommandHandler<TResult> typedPostHandler)
            {
                PostProcessItems.Enqueue(new OltAfterCommandQueueItem<TResult>(typedPostHandler, command, result));
            }
            else if (currentHandler is IOltPostCommandHandler postHandler)
            {
                PostProcessItems.Enqueue(new OltAfterCommandQueueItem(postHandler, command, (IOltCommandResult)result));
            }

            if (Context.Database.CurrentTransaction == null)
            {
                foreach (var item in PostProcessItems)
                {
                    await item.PostExecuteAsync(this);  //Run the nested command handlers in order
                }
            }
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
            return ExecuteAsync(GetHandler(command), command);
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
        // [Obsolete("ProcessAsync<T> is deprecated, use IOltCommand<TResult>")] //TODO: Mark as Obsolete
        public virtual async Task<T> ProcessAsync<T>(IOltCommand command)
        {
            var result = await ExecuteAsync(GetHandler(command), command);
            return result.CommandResult.GetResult<T>();
        }




        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="TResult"><see cref="IOltCommandHandler"/> Returned Result</typeparam>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="OltCommandResultNullException">Thrown is command result is null</exception>        
        /// <returns></returns>        
        public virtual async Task<TResult> ProcessAsync<TResult>(IOltCommand<TResult> command) where TResult : notnull
        {
            var validationResult = await ValidateAsync(command);
            if (!validationResult.Valid)
            {
                throw validationResult.ToException();
            }

            var handler = GetHandler(command);
            
            if (handler is IOltCommandHandler<TResult> typedResultHandler)
            {
                var result = await Context.Database.UsingDbTransactionAsync(async () =>
                {
                    return await typedResultHandler.ExecuteAsync(this, command);
                });

                await PostExecuteAsync<TResult>(handler, command, result);

                return result;
            }

            return await ProcessAsync<TResult>((IOltCommand)command);
        }
    }
}