using System;

namespace OLT.Core
{
    public abstract class OltRule : OltDisposable, IOltRule
    {
        public virtual string RuleName => this.GetType().FullName;
    }
}