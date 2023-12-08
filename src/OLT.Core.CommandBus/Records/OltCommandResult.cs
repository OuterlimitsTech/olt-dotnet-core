using System;

namespace OLT.Core
{

    public record OltCommandResult(object Result = null) : IOltCommandResult
    {
        public static OltCommandResult Complete()
        {
            return new OltCommandResult();
        }

        //[Obsolete("Complete<T> is deprecated, use IOltCommand<TResult> with OltCommandHandler<TCommand, TResult>")]
        public static OltCommandResult Complete<T>(T result)  //TODO: Mark as Obsolete
        {
            return new OltCommandResult(result);
        }

        /// <summary>
        /// Attempts to cast result to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="OltCommandResultNullException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public T GetResult<T>()
        {
            return OltCommandExtensions.ToResult<T>(this);
        }
    }
}