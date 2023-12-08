using System;
using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltCommandBus : IOltInjectableScoped
    {

        /// <summary>
        /// Runs Command Validation
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="OltCommandHandlerMultipleException"></exception>
        Task<IOltCommandValidationResult> ValidateAsync(IOltCommand command);

        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="OltCommandHandlerMultipleException"></exception>
        Task ProcessAsync(IOltCommand command);


        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="OltCommandResultNullException">Thrown is command result is null</exception>
        /// <exception cref="InvalidCastException">Thrown if command result can not be cast to <typeparamref name="T"/></exception>
        // [Obsolete("ProcessAsync<T> is deprecated, use IOltCommand<TResult>")]  //TODO: Mark as Obsolete
        Task<T> ProcessAsync<T>(IOltCommand command);

        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="TResult"><see cref="IOltCommandHandler"/> Returned Result</typeparam>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="OltCommandResultNullException">Thrown is command result is null</exception>        
        /// <returns></returns>
        Task<TResult> ProcessAsync<TResult>(IOltCommand<TResult> command) where TResult : notnull;
    }
}