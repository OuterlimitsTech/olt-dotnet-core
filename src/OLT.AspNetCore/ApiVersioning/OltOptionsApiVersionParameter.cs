using Microsoft.AspNetCore.Mvc.Versioning;
using OLT.Constants;
using System.Collections.Generic;

namespace OLT.Core
{
    public class OltOptionsApiVersionParameter
    {
        /// <summary>
        /// Reads the API Version from the query string <see cref="MediaTypeApiVersionReader"/>
        /// </summary>
        /// <remarks>
        /// Default <see cref="OltAspNetCoreDefaults.ApiVersion.ParameterName.Query"/>
        /// </remarks>
        public virtual string Query { get; set; } = OltAspNetCoreDefaults.ApiVersion.ParameterName.Query;

        /// <summary>
        /// Reads the API Version from media type <see cref="MediaTypeApiVersionReader"/>
        /// </summary>
        /// <remarks>
        /// Default <see cref="OltAspNetCoreDefaults.ApiVersion.ParameterName.MediaType"/>
        /// </remarks>
        public virtual string MediaType { get; set; } = OltAspNetCoreDefaults.ApiVersion.ParameterName.MediaType;

        /// <summary>
        /// Reads the API Version from media type <see cref="MediaTypeApiVersionReader"/>
        /// </summary>
        /// <remarks>
        /// Default <see cref="OltAspNetCoreDefaults.ApiVersion.ParameterName.Header"/>
        /// </remarks>
        public virtual string Header { get; set; } = OltAspNetCoreDefaults.ApiVersion.ParameterName.Header;



        public virtual List<IApiVersionReader> BuildReaders()
        {
            var readers = new List<IApiVersionReader>();
            if (!string.IsNullOrWhiteSpace(Query))
            {
                readers.Add(new QueryStringApiVersionReader(Query));
            }
            else
            {
                readers.Add(new QueryStringApiVersionReader());
            }

            if (!string.IsNullOrWhiteSpace(MediaType))
            {
                readers.Add(new MediaTypeApiVersionReader(MediaType));
            }
            else
            {
                readers.Add(new MediaTypeApiVersionReader());
            }

            if (!string.IsNullOrWhiteSpace(Header))
            {
                readers.Add(new HeaderApiVersionReader(Header));
            }
            else
            {
                readers.Add(new HeaderApiVersionReader(OltAspNetCoreDefaults.ApiVersion.ParameterName.Header));
            }

            readers.Add(new UrlSegmentApiVersionReader());

            return readers;
        }


    }
}