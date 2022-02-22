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
        /// Whitelist
        /// </summary>
        /// <param name="value"><see cref="OltEmailConfigurationWhitelist"/></param>
        /// <returns><typeparamref name="T"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T WithWhitelist(OltEmailConfigurationWhitelist value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            this.Whitelist = value;
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

            return Whitelist.Domain?.Any(p => emailAddress.EndsWith(p, StringComparison.OrdinalIgnoreCase)) == true ||
                   Whitelist.Email?.Any(p => emailAddress.Equals(p, StringComparison.OrdinalIgnoreCase)) == true;
        }

    }

}
