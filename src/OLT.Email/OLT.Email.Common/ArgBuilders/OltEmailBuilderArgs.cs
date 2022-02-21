using System;
using System.Collections.Generic;
using System.Text;

namespace OLT.Email
{

    public abstract class OltEmailBuilderArgs
    {

        // <summary>
        // Production Enabled
        // </summary>
        protected internal abstract bool Enabled { get; }

        // <summary>
        // Determines if Email can be sent depending on whitelist or production
        // </summary>
        public abstract bool AllowSend(string emailAddress);


        protected virtual List<string> Validate()
        {
            return new List<string>();
        }
    }
}
