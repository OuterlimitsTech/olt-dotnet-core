using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Logging.Serilog
{
    public static class OltNgxLoggerExtensions
    {
        /// <summary>
        /// Converts ngx-logger <see href="https://www.npmjs.com/package/ngx-logger"/> log level to <see cref="LogLevel"/>
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
