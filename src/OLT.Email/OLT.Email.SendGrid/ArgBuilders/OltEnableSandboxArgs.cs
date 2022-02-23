using System;

namespace OLT.Email.SendGrid
{
    public abstract class OltEnableSandboxArgs<T> : OltCustomArgsArgs<T>
        where T : OltEnableSandboxArgs<T>
    {
        protected internal bool SandboxMode { get; set; }

        protected OltEnableSandboxArgs()
        {
        }

        /// <summary>
        /// Enables Sandbox mode in Sendgrid message
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T EnableSandbox()
        {
            this.SandboxMode = true;
            return (T)this;
        }
    }
}
