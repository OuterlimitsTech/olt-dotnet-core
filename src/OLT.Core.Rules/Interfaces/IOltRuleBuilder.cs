using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltRuleBuilder : IOltRule
    {
        List<OltRuleCanRunException> CanExecute();
        Task<IOltRuleResult> ExecuteAsync(IDbContextTransaction dbTransaction);
    }
}