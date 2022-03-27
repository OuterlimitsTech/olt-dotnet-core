using System.Diagnostics;
using System.Linq;
using OLT.Constants;
using OLT.Logging.Serilog.Enricher;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace OLT.Logging.Serilog
{
    public static class OltSerilogExtensions
    {

        /// <summary>
        /// Enrich log unique identifier using <see cref="OltEventTypeEnricher"/>.
        /// </summary>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration WithOltEventType(this LoggerEnrichmentConfiguration loggerConfiguration)
        {
            return loggerConfiguration
                .With(new OltEventTypeEnricher());
        }


        /// <summary>
        /// Enrich log the Environment Name <see cref="OltSerilogConstants.Properties.Environment"/> and <see cref="OltSerilogConstants.Properties.DebuggerAttached"/>
        /// </summary>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration WithOltEnvironment(this LoggerEnrichmentConfiguration loggerConfiguration, string environmentName)
        {
            return loggerConfiguration
                .WithProperty(OltSerilogConstants.Properties.Environment, environmentName)
                // Used to filter out potentially bad data due debugging.
                // Very useful when doing Seq dashboards and want to remove logs under debugging session.
                .Enrich.WithProperty(OltSerilogConstants.Properties.DebuggerAttached, Debugger.IsAttached);
        }



        /// <summary>
        /// Writes <see cref="OltNgxLoggerMessageJson"/> to log
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="model"></param>
        public static void Write(this ILogger logger, OltNgxLoggerMessageJson model)
        {
            var level = model.Level?.ToLogLevel() ?? LogEventLevel.Information;

            if (level == LogEventLevel.Error)
            {
                logger
                    .ForContext(OltSerilogConstants.Properties.NgxMessage.MessageAsJson, model, destructureObjects: true)
                    .Error(model.ToException(), OltSerilogConstants.Templates.NgxMessage.Template, model.FormatMessage());
                return;
            }

            if (level == LogEventLevel.Fatal)
            {
                logger
                    .ForContext(OltSerilogConstants.Properties.NgxMessage.MessageAsJson, model, destructureObjects: true)
                    .Fatal(model.ToException(), OltSerilogConstants.Templates.NgxMessage.Template, model.FormatMessage());
                return;
            }


            logger
                .ForContext(OltSerilogConstants.Properties.NgxMessage.MessageAsJson, model, destructureObjects: true)
                .Write(level, OltSerilogConstants.Templates.NgxMessage.Template, model.FormatMessage());            
        }
    }
}
