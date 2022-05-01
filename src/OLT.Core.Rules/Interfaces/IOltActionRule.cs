using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltActionRule : IOltRule
    {
        List<OltRuleCanRunException> CanExecute();
        Task ExecuteAsync(IDbContextTransaction dbTransaction);
    }
}