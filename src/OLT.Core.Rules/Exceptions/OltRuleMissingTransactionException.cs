using Microsoft.EntityFrameworkCore.Storage;

namespace OLT.Core
{
    public class OltRuleMissingTransactionException : OltRuleCanRunException
    {
        public OltRuleMissingTransactionException(IOltRule rule) : base($"{rule.RuleName} requires {nameof(IDbContextTransaction)}")
        {
        }
    }
}