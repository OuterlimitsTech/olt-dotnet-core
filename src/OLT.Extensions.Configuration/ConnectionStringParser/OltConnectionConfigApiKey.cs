using System.Data.Common;

namespace OLT.Core
{
    /// <summary>
    /// API Key Connection String 
    /// </summary>
    /// <remarks>
    /// To parse, pass string to <seealso cref="Parse(string)"/>
    /// <list type="table">
    ///   <item>
    ///     <term>Connection String</term>
    ///     <description>endpoint=https://api.domain.com;apikey=API_KEY_HERE</description>
    ///   </item>
    ///   <item>
    ///     <term>Code Examples</term>
    ///     <description><a href="https://github.com/OuterlimitsTech/olt-dotnet-core/tree/ca433264489349ecf35aed6bfb3275feac8d3670/src/OLT.Extensions.Configuration"/></description>
    ///   </item>    
    /// </list>
    /// </remarks>
    public class OltConnectionConfigApiKey : IOltConnectionStringParser
    {
        /// <summary>
        /// key "endpoint"
        /// </summary>
        public string? Endpoint { get; set; }

        /// <summary>
        /// key "apikey"
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// Parses <paramref name="connString"/> [ endpoint=https://api.domain.com;apikey=APK_KEY_HERE; ]
        /// </summary>
        /// <param name="connString"></param>    
        public void Parse(string? connString)
        {
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder
            {
                ConnectionString = connString
            };
            Endpoint = builder.ContainsKey("endpoint") ? builder["endpoint"].ToString() : null;
            ApiKey = builder.ContainsKey("apikey") ? builder["apikey"].ToString() : null;

            if (Endpoint?.EndsWith("/") == false)
            {
                Endpoint = $"{Endpoint}/";
            }

        }
    }
}
