using System;
using System.Collections.Generic;
using System.Text;

namespace OLT.Email
{
    public interface IEmailBuilderArgs
    {
        bool AllowSend(string emailAddress);
    }

    public abstract class OltEmailBuilderArgs
    {        
        protected internal abstract bool Enabled { get; }
        public abstract bool AllowSend(string emailAddress);

        protected virtual List<string> Validate()
        {
            return new List<string>();
        }
    }
}
