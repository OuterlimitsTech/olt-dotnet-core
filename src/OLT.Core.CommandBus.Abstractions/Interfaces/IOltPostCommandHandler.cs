using System.Threading.Tasks;

namespace OLT.Core
{
    /// <summary>
    /// Triggered after DB Transaction commit
    /// </summary>
    public interface IOltPostCommandHandler : IOltCommandHandler
    {
        /// <summary>
        /// Called after all Transactions are committed. Allows for Sending Emails, Submitting Event Queues, etc.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        Task PostExecuteAsync(IOltCommand command, IOltCommandResult result);
    }


    /// <summary>
    /// Triggered after DB Transaction commit
    /// </summary>
    public interface IOltPostCommandHandler<TResult> : IOltCommandHandler<TResult>
        where TResult: notnull
    {
        /// <summary>
        /// Called after all Transactions are committed. Allows for Sending Emails, Submitting Event Queues, etc.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        Task PostExecuteAsync(IOltCommand command, TResult result);
    }

}