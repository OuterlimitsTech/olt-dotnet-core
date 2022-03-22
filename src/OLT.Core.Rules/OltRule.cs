namespace OLT.Core
{
    public abstract class OltRule : OltDisposable, IOltRule
    {
        public virtual string RuleName => this.GetType().FullName;
        protected virtual IOltResult Success => OltRuleResultHelper.Success;
        protected virtual IOltResultValidation Valid => OltRuleResultHelper.Valid;
        protected virtual OltRuleException Failure(string message) => new OltRuleException(message);
    }
}