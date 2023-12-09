using System.Data.Common;

namespace OLT.Core
{
    /// <summary>
    /// WCF Connection String
    /// </summary>
    /// <remarks>
    /// To parse, pass string to <seealso cref="Parse(string?)"/>
    /// <list type="table">
    ///   <item>
    ///     <term>Connection String</term>
    ///     <description>endpoint=https://domain.com/wcf-service/service.svc;username=UsernameHere;password=PasswordHere</description>
    ///   </item>
    ///   <item>
    ///     <term>Code Examples</term>
    ///     <description><a href="https://github.com/OuterlimitsTech/olt-dotnet-core/tree/ca433264489349ecf35aed6bfb3275feac8d3670/src/OLT.Extensions.Configuration"/></description>
    ///   </item>    
    /// </list>
    /// </remarks>
    public class OltConnectionConfigWcf
    {
        public string? Endpoint { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        /// <summary>
        /// Parses <paramref name="connString"/> [ endpoint=https://domain.com/wcf-service/service.svc;username=UsernameHere;password=PasswordHere; ]
        /// </summary>
        /// <param name="connString"></param>
        public void Parse(string? connString)
        {
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder
            {
                ConnectionString = connString
            };
            Endpoint = builder.ContainsKey("endpoint") ? builder["endpoint"].ToString() : null;
            Username = builder.ContainsKey("username") ? builder["username"].ToString() : null;
            Password = builder.ContainsKey("password") ? builder["password"].ToString() : null;
        }
    }
}
