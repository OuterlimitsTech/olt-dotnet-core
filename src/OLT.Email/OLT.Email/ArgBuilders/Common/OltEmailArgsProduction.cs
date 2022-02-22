namespace OLT.Email
{
    public abstract class OltEmailArgsProduction<T> : OltEmailBuilderArgs
      where T : OltEmailArgsProduction<T>
    {
        private bool _enabled = false;

        protected internal override bool Enabled => _enabled;        

        protected OltEmailArgsProduction()
        {
        }

        /// <summary>
        /// Sends emails for all requests and regardless whitelist values
        /// </summary>
        /// <param name="value"><see cref="bool"/></param>
        /// <returns><typeparamref name="T"/></returns>
        public T EnableProductionEnvironment(bool value)
        {
            this._enabled = value;
            return (T)this;
        }
    } 


}
