using System;
using System.Linq;

namespace OLT.Email
{
    public abstract class OltEmailArgsWhitelist<T> : OltEmailArgsProduction<T>
        where T : OltEmailArgsWhitelist<T>
    {
        protected internal OltEmailConfigurationWhitelist Whitelist { get; set; } = new OltEmailConfigurationWhitelist();

        protected OltEmailArgsWhitelist()
        {
        }

        /// <summary>
        /// Adds emails and domains to whitelist
        /// </summary>
        /// <param name="config"><see cref="OltEmailConfigurationWhitelist"/></param>
        /// <returns><typeparamref name="T"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T WithWhitelist(OltEmailConfigurationWhitelist config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (config.Domain != null)
            {
                this.Whitelist.Domain.AddRange(config.Domain.Except(this.Whitelist.Domain));
            }
            
            if (config.Email != null)
            {
                this.Whitelist.Email.AddRange(config.Email.Except(this.Whitelist.Email));
            }
            
            return (T)this;
        }

        /// <summary>
        /// Adds email address to whitelist
        /// </summary>
        /// <param name="emailAddress"><see cref="IOltEmailAddress"/></param>
        /// <returns><typeparamref name="T"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">When email is null</exception>
        public T WithWhitelist(IOltEmailAddress emailAddress)
        {
            if (emailAddress == null)
            {
                throw new ArgumentNullException(nameof(emailAddress));
            }

            if (emailAddress.Email == null)
            {
                throw new InvalidOperationException($"{nameof(emailAddress)}.{nameof(emailAddress.Email)} is null");
            }

            if (!this.Whitelist.Email.Any(value => string.Equals(emailAddress.Email, value, StringComparison.OrdinalIgnoreCase)))
            {
                this.Whitelist.Email.Add(emailAddress.Email);
            }

            return (T)this;
        }

        /// <summary>
        /// Determines if Email can be sent depending on <see cref="Production"/> is true or <see cref="TestWhitelist"/> <see cref="Production"/> is false
        /// </summary>
        public override bool AllowSend(string emailAddress)
        {
            if (base.Enabled)
            {
                return true;
            }

            return Whitelist.Domain.Any(p => emailAddress.EndsWith(p, StringComparison.OrdinalIgnoreCase)) ||
                   Whitelist.Email.Any(p => emailAddress.Equals(p, StringComparison.OrdinalIgnoreCase));
        }

    }

}
