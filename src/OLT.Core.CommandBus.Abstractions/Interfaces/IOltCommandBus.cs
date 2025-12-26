using System;
using System.Threading;
using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltCommandBus : IOltInjectableScoped
    {

        /// <summary>
        /// Runs Command Validation
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="OltCommandHandlerMultipleException"></exception>
        Task<IOltCommandValidationResult> ValidateAsync(IOltCommand command, CancellationToken cancellationToken = default);

        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="OltCommandHandlerMultipleException"></exception>
        [Obsolete("Removing in 10.x, ProcessAsync<T> is deprecated, use IOltCommand<TResult>")]
        Task ProcessAsync(IOltCommand command, CancellationToken cancellationToken = default);


        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="NullReferenceException">Thrown is command result is null</exception>
        /// <exception cref="InvalidCastException">Thrown if command result can not be cast to <typeparamref name="T"/></exception>
        [Obsolete("Removing in 10.x, ProcessAsync<T> is deprecated, use IOltCommand<TResult>")]  //Issue #149
        Task<T> ProcessAsync<T>(IOltCommand command);

        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <typeparam name="TResult"><see cref="IOltCommandHandler"/> Returned Result</typeparam>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="NullReferenceException">Thrown is command result is null</exception>        
        /// <exception cref="InvalidCastException">Thrown is command result doesn't match handler type</exception>
        /// <returns></returns>
        Task<TResult> ProcessAsync<TResult>(IOltCommand<TResult> command, CancellationToken cancellationToken = default) where TResult : notnull;
    }
}