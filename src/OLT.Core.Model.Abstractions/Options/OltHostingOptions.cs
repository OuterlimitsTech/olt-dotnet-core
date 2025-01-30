namespace OLT.Core
{
    public class OltHostingOptions
    {
        /// <summary>
        /// Enables UseDeveloperExceptionPage();
        /// </summary>
        /// <remarks>
        /// This should only be enabled in the Development environment. 
        /// </remarks>
        /// <remarks>Default: false</remarks>
        public virtual bool ShowExceptionDetails { get; set; }
    }

   
}
