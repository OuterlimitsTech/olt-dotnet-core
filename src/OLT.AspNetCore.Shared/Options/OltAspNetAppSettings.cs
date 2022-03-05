namespace OLT.Core
{
    public class OltAspNetAppSettings : IOltOptionsAspNet
    {
        /// <summary>
        /// Hosting Settings
        /// </summary>
        public virtual OltAspNetHostingOptions Hosting { get; set; } = new OltAspNetHostingOptions();
        
    }
}