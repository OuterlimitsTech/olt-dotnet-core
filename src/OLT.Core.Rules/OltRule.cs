using System;

namespace OLT.Core
{
    [Obsolete("Move to OltCommandBus")]
    public abstract class OltRule : OltDisposable, IOltRule
    {
        public virtual string RuleName => this.GetType().FullName;
    }
}