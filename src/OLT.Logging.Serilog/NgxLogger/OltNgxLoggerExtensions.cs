
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Logging.Serilog
{
    public static class OltNgxLoggerExtensions
    {
        /// <summary>
        /// Converts ngx-logger <see href="https://www.npmjs.com/package/ngx-logger"/> log level to <see cref="LogEventLevel"/>
        /// </summary>
        /// <param name="ngxLoggerLevel"></param>
        /// <returns></returns>
        public static LogEventLevel ToLogLevel(this OltNgxLoggerLevel ngxLoggerLevel)
        {
            switch (ngxLoggerLevel)
            {
                case OltNgxLoggerLevel.Trace:
                    return LogEventLevel.Verbose;
                case OltNgxLoggerLevel.Debug:
                    return LogEventLevel.Debug;
                case OltNgxLoggerLevel.Information:
                    return LogEventLevel.Information;
                case OltNgxLoggerLevel.Log:
                    return LogEventLevel.Information;
                case OltNgxLoggerLevel.Warning:
                    return LogEventLevel.Warning;
                case OltNgxLoggerLevel.Error:
                    return LogEventLevel.Error;
                case OltNgxLoggerLevel.Fatal:
                    return LogEventLevel.Fatal;
                case OltNgxLoggerLevel.Off:
                    return LogEventLevel.Information;
                default:
                    return LogEventLevel.Information;
            }
        }

        public static string FormatStack(this OltNgxLoggerStackJson value)
        {
            var list = new List<string>
            {
                $"Column Number: {value.ColumnNumber}",
                $"Line Number: {value.LineNumber}",
                $"FileName: {value.FileName}",
                $"FunctionName: {value.FunctionName}",
                $"Source: {value.Source}",
            };
            return string.Join(Environment.NewLine, list);
        }

        public static string FormatStack(this List<OltNgxLoggerStackJson> stack)
        {
            return string.Join($"----------------------------{Environment.NewLine}", stack.Select(s => $"{FormatStack(s)}{Environment.NewLine}").ToList());
        }
    }
}
