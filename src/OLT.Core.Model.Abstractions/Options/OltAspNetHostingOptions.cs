namespace OLT.Core
{
    public class OltAspNetHostingOptions : OltHostingOptions
    {
        /// <summary>
        /// Adds middleware for using HSTS, which adds the Strict-Transport-Security header.
        /// </summary>
        public virtual bool UseHsts { get; set; }

        /// <summary>
        /// Adds a middleware that extracts the specified path base from request path and postpend it to the request path base.
        /// </summary>
        public virtual string? PathBase { get; set; }


        /// <summary>
        /// Disables middleware for redirecting HTTP Requests to HTTPS.
        /// </summary>
        public virtual bool DisableHttpsRedirect { get; set; }

    }

   
}
