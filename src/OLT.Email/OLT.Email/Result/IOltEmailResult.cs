using System.Collections.Generic;
using OLT.Core;

namespace OLT.Email
{
    public interface IOltEmailResult : IOltResult
    {
        List<string> Errors { get; set; }
        OltEmailRecipientResult RecipientResults { get; set; }
    }

}
