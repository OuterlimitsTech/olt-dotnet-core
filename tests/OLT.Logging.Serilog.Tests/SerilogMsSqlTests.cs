using FluentAssertions;
using Serilog;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using OLT.Constants;
using OLT.Core;
using System.Data;
using System;
using Serilog.Events;

namespace OLT.Logging.Serilog.Tests
{
    public class SerilogMsSqlTests
    {
        [Fact]
        public void DefaultColumnOptionTests()
        {
            var result = OltSerilogMsSqlExtensions.DefaultColumnOptions;
            result.AdditionalColumns.Should().HaveCount(7);
            result.AdditionalColumns.Should().Satisfy(
                e => e.ColumnName == OltSerilogMsSqlConstants.ColumnNames.Application,
                e => e.ColumnName == OltSerilogMsSqlConstants.ColumnNames.RequestPath,
                e => e.ColumnName == OltSerilogMsSqlConstants.ColumnNames.Source,
                e => e.ColumnName == OltSerilogMsSqlConstants.ColumnNames.EventType,
                e => e.ColumnName == OltSerilogMsSqlConstants.ColumnNames.UserPrincipalName,
                e => e.ColumnName == OltSerilogMsSqlConstants.ColumnNames.Username,
                e => e.ColumnName == OltSerilogMsSqlConstants.ColumnNames.DbUsername
                );

            Assert.Equal(OltSerilogMsSqlConstants.ColumnNames.Id, result.Id.ColumnName);
            Assert.Equal(SqlDbType.DateTimeOffset, result.TimeStamp.DataType);

            Assert.Equal("OltEventType", OltSerilogMsSqlConstants.ColumnNames.EventType);
            Assert.Equal("UserPrincipalName", OltSerilogMsSqlConstants.ColumnNames.UserPrincipalName);
            Assert.Equal("Username", OltSerilogMsSqlConstants.ColumnNames.Username);
            Assert.Equal("DbUsername", OltSerilogMsSqlConstants.ColumnNames.DbUsername);

        }

        [Fact]
        public void DefaultSinkOptionTests()
        {
            var result = OltSerilogMsSqlExtensions.DefaultSinkOptions;
            Assert.Equal(OltSerilogMsSqlConstants.Table.Name, result.TableName);
            Assert.Equal(OltSerilogMsSqlConstants.Table.Schema, result.SchemaName);            
        }



        [Fact]
        public void ExtensionTests()
        {
            var loggerConfig = new LoggerConfiguration();
            var connectionString = Faker.Lorem.GetFirstWord();

            Assert.Throws<ArgumentNullException>("loggerConfiguration", () => OltSerilogMsSqlExtensions.WithOltMSSqlServer(null, connectionString, null, null, LogEventLevel.Debug));

            try
            {
                OltSerilogMsSqlExtensions.WithOltMSSqlServer(loggerConfig, null, null, null, LogEventLevel.Debug);
            }
            catch (ArgumentException)
            {
                Assert.True(true);
            }

            try
            {
                OltSerilogMsSqlExtensions.WithOltMSSqlServer(loggerConfig, connectionString, null, null, LogEventLevel.Debug);
                var logger = loggerConfig.CreateLogger();
                logger.Debug("{value1}", Faker.Lorem.Words(10).Last());
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }

        }
    }
}