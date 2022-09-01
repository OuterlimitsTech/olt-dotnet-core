using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltActionRule : IOltRule
    {
        Task<List<OltRuleCanRunException>> CanExecuteAsync();
        Task ExecuteAsync(IDbContextTransaction dbTransaction);
    }
}