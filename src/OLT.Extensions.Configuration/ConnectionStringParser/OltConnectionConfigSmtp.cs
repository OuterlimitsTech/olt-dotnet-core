using System.Data.Common;

namespace OLT.Core
{
    /// <summary>
    /// SMTP Connection String
    /// </summary>
    /// <remarks>
    /// To parse, pass string to <seealso cref="Parse(string?)"/>
    /// <list type="table">
    ///   <item>
    ///     <term>Connection String</term>
    ///     <description>host=localhost;port=587;username=usernameHere;password=passwordHere;ssl=true;</description>
    ///   </item>
    ///   <item>
    ///     <term>Code Examples</term>
    ///     <description><a href="https://github.com/OuterlimitsTech/olt-dotnet-core/tree/ca433264489349ecf35aed6bfb3275feac8d3670/src/OLT.Extensions.Configuration"/></description>
    ///   </item>    
    /// </list>
    /// </remarks>
    public class OltConnectionConfigSmtp : OltConnectionConfigSftp
    {
        public override int Port { get; set; } = 587;
        public virtual bool? EnableSsl { get; set; }

        /// <summary>
        /// Parses <paramref name="connString"/> [ host=localhost;port=22;username=usernameHere;password=passwordHere; ]
        /// </summary>
        /// <remarks>
        ///   <list type="bullet">
        ///     <item><description><see cref="Port"/> is optional and will default to 587</description></item>        
        ///   </list>
        /// </remarks>
        /// <param name="connString"></param>
        public override void Parse(string? connString)
        {            
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder
            {
                ConnectionString = connString
            };
            Host = builder.ContainsKey("host") ? builder["host"].ToString() : null;
            Username = builder.ContainsKey("username") ? builder["username"].ToString() : null;
            Password = builder.ContainsKey("password") ? builder["password"].ToString() : null;
            Port = builder.ContainsKey("port") ? builder["port"].ToString().ToPort(this.Port) : this.Port;
            EnableSsl = builder.ContainsKey("ssl") ? builder["ssl"].ToString().ToBoolean(null) : null;
        }

        
    }
}
