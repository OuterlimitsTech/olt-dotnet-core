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
        OltEmailResult Send();
        Task<OltEmailResult> SendAsync();
    }

    public interface IOltEmailClient<out TClient, out TMessage> : IOltEmailClient
        where TClient : class
        where TMessage : class
    {        
        TClient CreateClient();
        TMessage CreateMessage(OltEmailRecipientResult recipients);
    }
}
