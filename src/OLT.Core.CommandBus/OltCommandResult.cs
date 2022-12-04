using System;

namespace OLT.Core
{

    public record OltCommandResult(object Result = null) : IOltCommandResult
    {
        public static OltCommandResult Complete()
        {
            return new OltCommandResult();
        }

        public static OltCommandResult Complete<T>(T result)
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
            if (Result == null)
            {
                throw new NullReferenceException($"{nameof(OltCommandResult)}.{nameof(Result)}");
            }

            if (Result.GetType() == typeof(T))
            {
                return (T)Result;
            }
            throw new InvalidCastException($"Unable cast {nameof(OltCommandResult)}.{nameof(Result)} to {typeof(T).FullName}");
        }
    }
}