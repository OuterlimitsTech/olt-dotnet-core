using OLT.Constants;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.Generic;
using System.Data;

namespace OLT.Core
{

    public static class OltSerilogMsSqlExtensions
    {

        public static LoggerConfiguration WithOltMSSqlServer(this LoggerConfiguration loggerConfiguration, string connectionString, MSSqlServerSinkOptions options = null, ColumnOptions columnOptions = null, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Information)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                options = options ?? DefaultSinkOptions;
                columnOptions = columnOptions ?? DefaultColumnOptions;

                loggerConfiguration
                    .WriteTo.MSSqlServer(
                        connectionString,
                        restrictedToMinimumLevel: restrictedToMinimumLevel,
                        sinkOptions: options,
                        columnOptions: columnOptions);
            }

            return loggerConfiguration;
        }

        public static MSSqlServerSinkOptions DefaultSinkOptions
        {
            get
            {
                return new MSSqlServerSinkOptions
                {
                    TableName = "Log",
                    SchemaName = "dbo",
                    AutoCreateSqlTable = true,
                };
            }
        }

        public static ColumnOptions DefaultColumnOptions
        {
            get
            {
                var additionalColumns = new List<SqlColumn>
                {
                    new SqlColumn
                        {ColumnName = "Application", PropertyName = "Application", DataType = SqlDbType.NVarChar, DataLength = 100},

                    new SqlColumn
                        {ColumnName = OltSerilogConstants.Properties.EventType, PropertyName = OltSerilogConstants.Properties.EventType, DataType = SqlDbType.NVarChar, DataLength = 20},

                    new SqlColumn
                        {ColumnName = OltSerilogConstants.Properties.UserPrincipalName, DataType = SqlDbType.NVarChar, DataLength = 25},

                    new SqlColumn
                        {ColumnName = OltSerilogConstants.Properties.Username, DataType = SqlDbType.NVarChar, DataLength = 255},

                    new SqlColumn
                        {ColumnName = OltSerilogConstants.Properties.DbUsername, DataType = SqlDbType.NVarChar, DataLength = 100},

                    new SqlColumn
                        {ColumnName = "RequestPath", DataType = SqlDbType.NVarChar, DataLength = -1},

                    new SqlColumn
                        {ColumnName = "Source", DataType = SqlDbType.NVarChar, DataLength = 50},
                };

                return new ColumnOptions
                {
                    Id = { ColumnName = "LogId" },
                    TimeStamp = { DataType = SqlDbType.DateTimeOffset },
                    AdditionalColumns = additionalColumns
                };
            }
        }
    }


}
