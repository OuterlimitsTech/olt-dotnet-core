using System;

namespace OLT.Core
{
    public static class OltCommandExtensions
    {
        /// <summary>
        /// Attempts to cast <see cref="OltCommandResult.Result"/> to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandResult"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public static T ToResult<T>(this IOltCommandResult commandResult)
        {
            return ToResult<T>((OltCommandResult)commandResult);
        }

        /// <summary>
        /// Attempts to cast <see cref="OltCommandResult.Result"/> to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandResult"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public static T ToResult<T>(this OltCommandResult commandResult)
        {
            if (commandResult?.Result == null)
            {
                throw new NullReferenceException();
            }

            if (commandResult.Result.GetType() == typeof(T))
            {
                return (T)commandResult.Result;
            }
            throw new InvalidCastException($"Unable cast {nameof(OltCommandResult)}.{nameof(commandResult.Result)} to {typeof(T).FullName}");
        }
    }
}