namespace OLT.Email.SendGrid
{
    public abstract class OltUnsubscribeGroupArgs<T> : OltDisableClickTrackingArgs<T>
        where T : OltUnsubscribeGroupArgs<T>
    {
        protected internal int? UnsubscribeGroupId { get; set; }

        protected OltUnsubscribeGroupArgs()
        {
        }

        /// <summary>
        /// Send Grid Template
        /// </summary>
        /// <returns></returns>
        public T WithUnsubscribeGroupId(int value)
        {
            this.UnsubscribeGroupId = value;
            return (T)this;
        }

     
    }
}
