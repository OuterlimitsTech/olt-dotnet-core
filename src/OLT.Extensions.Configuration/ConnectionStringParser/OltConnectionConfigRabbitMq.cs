using System.Data.Common;

namespace OLT.Core
{
    /// <summary>
    /// RabbitMq Connection String
    /// </summary>
    /// <remarks>    
    /// To parse, pass string to <seealso cref="Parse(string)"/>
    /// <list type="table">
    ///   <item>
    ///     <term>Connection String</term>
    ///     <description>host=rabbitmq://localhost:5672;username=guest;password=guest</description>
    ///   </item>
    ///   <item>
    ///     <term>Code Examples</term>
    ///     <description><a href="https://github.com/OuterlimitsTech/olt-dotnet-core/tree/ca433264489349ecf35aed6bfb3275feac8d3670/src/OLT.Extensions.Configuration"/></description>
    ///   </item>
    /// </list>
    /// </remarks>
    public class OltConnectionConfigRabbitMq : IOltConnectionStringParser
    {

        /// <summary>
        /// key "host"
        /// </summary>
        public virtual string? Host { get; set; }

        /// <summary>
        /// key "username"
        /// </summary>
        public virtual string? Username { get; set; }

        /// <summary>
        /// key "password"
        /// </summary>
        public virtual string? Password { get; set; }

        /// <summary>
        /// Parses <paramref name="connString"/> [ host=rabbitmq://localhost:5672;username=UsernameHere;password=PasswordHere; ]
        /// </summary>
        /// <param name="connString"></param>
        public void Parse(string? connString)
        {
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder
            {
                ConnectionString = connString
            };
            Host = builder.ContainsKey("host") ? builder["host"].ToString() : null;
            Username = builder.ContainsKey("username") ? builder["username"].ToString() : null;
            Password = builder.ContainsKey("password") ? builder["password"].ToString() : null;
        }
    }



}
