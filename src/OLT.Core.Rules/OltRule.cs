using System;

namespace OLT.Core
{
    public abstract class OltRule : OltDisposable, IOltRule
    {
        public virtual string RuleName => this.GetType().FullName;
        protected virtual IOltRuleResult Success => OltRuleResultHelper.Success;
        protected virtual IOltRuleResult InValid => OltRuleResultHelper.Invalid;
        protected virtual IOltRuleResult BadRequest(string message) => new OltRuleResultInvalid(new OltValidationError(message));
        protected virtual OltRuleException Failure(string message, Exception ex = null) => new OltRuleException(message, ex);
    }
}