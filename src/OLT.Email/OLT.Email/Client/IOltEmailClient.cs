using System.Collections.Generic;
using System.Threading.Tasks;

namespace OLT.Email
{
    public interface IOltEmailClient
    {
        bool AllowSend(string emailAddress);
        bool IsValid { get; }
        List<string> ValidationErrors();
        OltEmailRecipientResult BuildRecipients();
    }

    public interface IOltEmailClient<out TClient, out TMessage, TResult> : IOltEmailClient
        where TClient : class
        where TMessage : class
        where TResult : class, IOltEmailResult
    {        
        TClient CreateClient();
        TMessage CreateMessage(OltEmailRecipientResult recipients);
        TResult Send();
        Task<TResult> SendAsync();

    }
}
