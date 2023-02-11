using Asp.Versioning;
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
        /// Default <see cref="OltAspNetCoreVersioningDefaults.ApiVersion.ParameterName.Query"/>
        /// </remarks>
        public virtual string Query { get; set; } = OltAspNetCoreVersioningDefaults.ApiVersion.ParameterName.Query;

        /// <summary>
        /// Reads the API Version from media type <see cref="MediaTypeApiVersionReader"/>
        /// </summary>
        /// <remarks>
        /// Default <see cref="OltAspNetCoreVersioningDefaults.ApiVersion.ParameterName.MediaType"/>
        /// </remarks>
        public virtual string MediaType { get; set; } = OltAspNetCoreVersioningDefaults.ApiVersion.ParameterName.MediaType;

        /// <summary>
        /// Reads the API Version from media type <see cref="MediaTypeApiVersionReader"/>
        /// </summary>
        /// <remarks>
        /// Default <see cref="OltAspNetCoreVersioningDefaults.ApiVersion.ParameterName.Header"/>
        /// </remarks>
        public virtual string Header { get; set; } = OltAspNetCoreVersioningDefaults.ApiVersion.ParameterName.Header;



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
                readers.Add(new HeaderApiVersionReader(OltAspNetCoreVersioningDefaults.ApiVersion.ParameterName.Header));
            }

            readers.Add(new UrlSegmentApiVersionReader());

            return readers;
        }


    }
}