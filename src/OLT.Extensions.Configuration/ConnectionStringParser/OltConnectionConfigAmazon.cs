using System.Data.Common;

namespace OLT.Core
{
    /// <summary>
    /// AWS Connection String 
    /// </summary>
    /// <remarks>
    /// To parse, pass string to <seealso cref="Parse(string)"/>
    /// <list type="table">
    ///   <item>
    ///     <term>Connection String</term>
    ///     <description>region=us-west-2;accessKey=ACCESS_KEY_HERE;secretKey="SECRET_KEY_HERE</description>
    ///   </item>
    ///   <item>
    ///     <term>Code Examples</term>
    ///     <description><a href="https://github.com/OuterlimitsTech/olt-dotnet-core/tree/ca433264489349ecf35aed6bfb3275feac8d3670/src/OLT.Extensions.Configuration"/></description>
    ///   </item>    
    /// </list>
    /// </remarks>
    public class OltConnectionConfigAmazon : IOltConnectionStringParser
    {

        /// <summary>
        /// key "region"
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// key "accessKey"
        /// </summary>

        public string? AccessKey { get; set; }

        /// <summary>
        /// key "secretKey"
        /// </summary>
        public string? SecretKey { get; set; }

        /// <summary>
        /// Parses <paramref name="connString"/> [ host=rabbitmq://localhost:5672;username=UserNameHere;password=PasswordHere; ]
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="defaultRegion">example: us-east-2,us-west-1</param>
        public void Parse(string? connString, string? defaultRegion)
        {
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder
            {
                ConnectionString = connString
            };
            Region = builder.ContainsKey("region") ? builder["region"].ToString() : defaultRegion;
            AccessKey = builder.ContainsKey("accessKey") ? builder["accessKey"].ToString() : null;
            SecretKey = builder.ContainsKey("secretKey") ? builder["secretKey"].ToString() : null;
        }

        public void Parse(string? connString)
        {
            Parse(connString, null);
        }


    }



}
