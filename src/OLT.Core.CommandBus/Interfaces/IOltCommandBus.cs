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
        Task<IOltCommandValidationResult> ValidateAsync(IOltCommand command);

        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        Task ProcessAsync(IOltCommand command);


        /// <summary>
        /// Processes Command using <see cref="IOltCommandHandler"/> for <see cref="IOltCommand"/>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="OltCommandHandlerNotFoundException"></exception>
        /// <exception cref="NullReferenceException">Thrown is command result is null</exception>
        /// <exception cref="InvalidCastException">Thrown if command result can not be cast to <typeparamref name="T"/></exception>
        Task<T> ProcessAsync<T>(IOltCommand command);
    }
}