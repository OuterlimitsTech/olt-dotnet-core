using System;

namespace OLT.Core
{
    [Obsolete("Removing in 10.x, Complete<T> is deprecated, use IOltCommand<TResult> with OltCommandHandler<TCommand, TResult>")]
    public record OltCommandResult(object? Result = null) : IOltCommandResult
    {
        public static OltCommandResult Complete()
        {
            return new OltCommandResult();
        }

        
        public static OltCommandResult Complete<T>(T result)  //Issue #149
        {
            return new OltCommandResult(result);
        }

        /// <summary>
        /// Attempts to cast result to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public T GetResult<T>()
        {
            return OltCommandExtensions.ToResult<T>(this);
        }
    }


}