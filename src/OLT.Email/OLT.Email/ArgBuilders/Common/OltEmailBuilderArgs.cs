using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email
{

    public abstract class OltEmailBuilderArgs : IOltEmailClient 
    {
        
        // <summary>
        // Production Enabled
        // </summary>
        protected  abstract bool Enabled { get; }

        // <summary>
        // Determines if Email can be sent depending on whitelist or production
        // </summary>
        public abstract bool AllowSend(string emailAddress);

        public virtual bool IsValid => !ValidationErrors().Any();

        public virtual List<string> ValidationErrors()
        {
            return new List<string>();
        }

        public abstract OltEmailRecipientResult BuildRecipients();        
        
    }
}
