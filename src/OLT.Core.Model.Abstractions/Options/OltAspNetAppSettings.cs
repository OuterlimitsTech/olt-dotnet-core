namespace OLT.Core
{
    [Obsolete("Removing in 9.x")]
    public class OltAspNetAppSettings
    {
        /// <summary>
        /// Hosting Settings
        /// </summary>
        public virtual OltAspNetHostingOptions Hosting { get; set; } = new OltAspNetHostingOptions();

    }

   
}
