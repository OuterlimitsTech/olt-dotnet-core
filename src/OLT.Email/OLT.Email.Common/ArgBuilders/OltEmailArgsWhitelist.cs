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
        public T WithWhitelist(OltEmailConfigurationWhitelist value)
        {
            this.Whitelist = value;
            return (T)this;
        }
    }


   


}
