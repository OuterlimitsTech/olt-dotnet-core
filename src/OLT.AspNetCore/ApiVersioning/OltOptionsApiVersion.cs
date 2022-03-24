using Microsoft.AspNetCore.Mvc.Versioning;
using OLT.Constants;
using System.Collections.Generic;

namespace OLT.Core
{
    public class OltOptionsApiVersion 
    {

        /// <summary>
        /// Indicating whether a default version is assumed when a client does not provide a service API version.
        /// </summary>
        /// <remarks>
        /// Default true
        /// </remarks>
        public bool AssumeDefaultVersionWhenUnspecified { get; set; } = true;

        public OltOptionsApiVersionParameter Parameter { get; set; } = new OltOptionsApiVersionParameter();

    }


    public class OltOptionsApiVersionParameter
    {
        /// <summary>
        /// Reads the API Version from the query string <see cref="MediaTypeApiVersionReader"/>
        /// </summary>
        /// <remarks>
        /// Default <see cref="OltAspNetCoreDefaults.ApiVersion.ParameterName.Query"/>
        /// </remarks>
        public string Query { get; set; } = OltAspNetCoreDefaults.ApiVersion.ParameterName.Query;

        /// <summary>
        /// Reads the API Version from media type <see cref="MediaTypeApiVersionReader"/>
        /// </summary>
        /// <remarks>
        /// Default <see cref="OltAspNetCoreDefaults.ApiVersion.ParameterName.MediaType"/>
        /// </remarks>
        public string MediaType { get; set; } = OltAspNetCoreDefaults.ApiVersion.ParameterName.MediaType;

        /// <summary>
        /// Reads the API Version from media type <see cref="MediaTypeApiVersionReader"/>
        /// </summary>
        /// <remarks>
        /// Default <see cref="OltAspNetCoreDefaults.ApiVersion.ParameterName.Header"/>
        /// </remarks>
        public string Header { get; set; } = OltAspNetCoreDefaults.ApiVersion.ParameterName.Header;



        public List<IApiVersionReader> BuildReaders()
        {
            var readers = new List<IApiVersionReader>();
            return  readers;
        }


    }
}