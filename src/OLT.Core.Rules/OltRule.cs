using System;

namespace OLT.Core
{
    public abstract class OltRule : OltDisposable, IOltRule
    {
        public virtual string RuleName => this.GetType().FullName;

        [Obsolete("Move away from Results and Throw Exceptions")]
        protected virtual IOltRuleResult Success => OltRuleResultHelper.Success;
        [Obsolete("Move away from Results and Throw Exceptions")]
        protected virtual IOltRuleResult InValid => OltRuleResultHelper.Invalid;
        [Obsolete("Move away from Results and Throw Exceptions")]
        protected virtual IOltRuleResult BadRequest(string message) => new OltRuleResultInvalid(new OltValidationError(message));
        [Obsolete("Move away from Results and Throw Exceptions")]
        protected virtual OltRuleException Failure(string message, Exception ex = null) => new OltRuleException(message, ex);
    }
}