using System.Data.Common;

namespace OLT.Core
{
    /// <summary>
    /// SFTP Connection String
    /// </summary>
    /// <remarks>
    /// To parse, pass string to <seealso cref="Parse(string?)"/>
    /// <list type="table">
    ///   <item>
    ///     <term>Connection String</term>
    ///     <description>host=localhost;port=22;username=usernameHere;password=passwordHere;workingdir=/root/test</description>
    ///   </item>
    ///   <item>
    ///     <term>Code Examples</term>
    ///     <description><a href="https://github.com/OuterlimitsTech/olt-dotnet-core/tree/ca433264489349ecf35aed6bfb3275feac8d3670/src/OLT.Extensions.Configuration"/></description>
    ///   </item>    
    /// </list>
    /// </remarks>
    public class OltConnectionConfigSftp
    {
        public virtual string? Host { get; set; }
        public virtual string? Username { get; set; }
        public virtual string? Password { get; set; }
        public virtual int Port { get; set; } = 22;
        public virtual string? WorkingDirectory { get; set; }

        /// <summary>
        /// Parses <paramref name="connString"/> [ host=localhost;port=22;username=usernameHere;password=passwordHere;workingdir=/root/test ]
        /// </summary>
        /// <remarks>
        ///   <list type="bullet">
        ///     <item><description><see cref="Port"/> is optional and will default to 22</description></item>
        ///     <item><description><see cref="WorkingDirectory"/> is optional</description></item>
        ///   </list>
        /// </remarks>
        /// <param name="connString"></param>
        public virtual void Parse(string? connString)
        {
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder
            {
                ConnectionString = connString
            };
            Host = builder.ContainsKey("host") ? builder["host"].ToString() : null;
            Username = builder.ContainsKey("username") ? builder["username"].ToString() : null;
            Password = builder.ContainsKey("password") ? builder["password"].ToString() : null;
            Port = builder.ContainsKey("port") ? builder["port"].ToString().ToPort(this.Port) : this.Port;
            WorkingDirectory = builder.ContainsKey("workingdir") ? builder["workingdir"].ToString() : null;
        }
     
    }
}
