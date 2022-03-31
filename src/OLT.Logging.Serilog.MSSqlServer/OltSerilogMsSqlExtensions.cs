using System;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.Generic;
using System.Data;
using OLT.Constants;

namespace OLT.Core
{

    public static class OltSerilogMsSqlExtensions
    {

        public static LoggerConfiguration WithOltMSSqlServer(this LoggerConfiguration loggerConfiguration, string connectionString, MSSqlServerSinkOptions options = null, ColumnOptions columnOptions = null, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Information)
        {
            if (loggerConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            options = options ?? DefaultSinkOptions;
            columnOptions = columnOptions ?? DefaultColumnOptions;

            loggerConfiguration
                .WriteTo.MSSqlServer(
                    connectionString,
                    restrictedToMinimumLevel: restrictedToMinimumLevel,
                    sinkOptions: options,
                    columnOptions: columnOptions);

            return loggerConfiguration;
        }

        public static MSSqlServerSinkOptions DefaultSinkOptions
        {
            get
            {
                return new MSSqlServerSinkOptions
                {
                    TableName = OltSerilogMsSqlConstants.Table.Name,
                    SchemaName = OltSerilogMsSqlConstants.Table.Schema,
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
                        {ColumnName = OltSerilogMsSqlConstants.ColumnNames.Application, PropertyName = OltSerilogConstants.Properties.Application, DataType = SqlDbType.NVarChar, DataLength = 100},

                    new SqlColumn
                        {ColumnName = OltSerilogMsSqlConstants.ColumnNames.EventType, PropertyName = OltSerilogConstants.Properties.EventType, DataType = SqlDbType.NVarChar, DataLength = 20},

                    new SqlColumn
                        {ColumnName = OltSerilogMsSqlConstants.ColumnNames.UserPrincipalName, DataType = SqlDbType.NVarChar, DataLength = 25},

                    new SqlColumn
                        {ColumnName = OltSerilogMsSqlConstants.ColumnNames.Username, DataType = SqlDbType.NVarChar, DataLength = 255},

                    new SqlColumn
                        {ColumnName = OltSerilogMsSqlConstants.ColumnNames.DbUsername, DataType = SqlDbType.NVarChar, DataLength = 100},

                    new SqlColumn
                        {ColumnName = OltSerilogMsSqlConstants.ColumnNames.RequestPath, DataType = SqlDbType.NVarChar, DataLength = -1},

                    new SqlColumn
                        {ColumnName = OltSerilogMsSqlConstants.ColumnNames.Source, DataType = SqlDbType.NVarChar, DataLength = 50},
                };

                return new ColumnOptions
                {
                    Id = { ColumnName = OltSerilogMsSqlConstants.ColumnNames.Id },
                    TimeStamp = { DataType = SqlDbType.DateTimeOffset },
                    AdditionalColumns = additionalColumns
                };
            }
        }
    }


}
