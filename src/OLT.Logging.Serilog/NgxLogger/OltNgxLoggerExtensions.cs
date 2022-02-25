using Microsoft.Extensions.Logging;

namespace OLT.Logging.Serilog
{
    public static class OltNgxLoggerExtensions
    {
        /// <summary>
        /// Converts ngx
        /// </summary>
        /// <param name="ngxLoggerLevel"></param>
        /// <returns></returns>
        public static LogLevel ToLogLevel(this OltNgxLoggerLevel ngxLoggerLevel)
        {
            switch (ngxLoggerLevel)
            {
                case OltNgxLoggerLevel.Trace:
                    return LogLevel.Trace;
                case OltNgxLoggerLevel.Debug:
                    return LogLevel.Debug;
                case OltNgxLoggerLevel.Information:
                    return LogLevel.Information;
                case OltNgxLoggerLevel.Log:
                    return LogLevel.Information;
                case OltNgxLoggerLevel.Warning:
                    return LogLevel.Warning;
                case OltNgxLoggerLevel.Error:
                    return LogLevel.Error;
                case OltNgxLoggerLevel.Fatal:
                    return LogLevel.Critical;
                case OltNgxLoggerLevel.Off:
                    return LogLevel.None;
                default:
                    return LogLevel.Information;
            }
        }
    }
}
