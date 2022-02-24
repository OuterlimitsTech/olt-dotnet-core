namespace OLT.Email.SendGrid
{        
    public abstract class OltDisableClickTrackingArgs<T> : OltApiKeyArgs<T>
      where T : OltDisableClickTrackingArgs<T>
    {
        protected bool ClickTracking { get; set; } = true;

        protected OltDisableClickTrackingArgs()
        {
        }

        /// <summary>
        /// Disables SendGrid's click tracking
        /// </summary>
        /// <returns></returns>
        public T WithoutClickTracking()
        {
            this.ClickTracking = false;
            return (T)this;
        }
    }
}
