using Faker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Logging.Serilog.Tests.NgxLogger
{
    public class HelperNgxExceptionTest
    {
        public HelperNgxExceptionTest(DateTimeOffset? dt)
        {
            if (dt.HasValue)
            {
                Timestamp = dt.Value;
                UnixTime = dt.Value.ToUnixTimeMilliseconds();
            }


            Stack = new List<OltNgxLoggerStackJson>
            {
                new OltNgxLoggerStackJson
                {
                    ColumnNumber = Faker.RandomNumber.Next(3),
                    LineNumber = Faker.RandomNumber.Next(4),
                    FileName = Faker.Lorem.GetFirstWord(),
                    FunctionName = Faker.Lorem.GetFirstWord(),
                    Source = Faker.Lorem.Paragraph()
                }
            };


            Result = new Dictionary<string, string>
            {
                { "Name", Faker.Name.FullName(NameFormats.WithSuffix) },
                { "AppId", Faker.Lorem.GetFirstWord() },
                { "User", Faker.Internet.UserName() },
                { "Time", UnixTime.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(UnixTime.Value).ToString(OltSerilogConstants.FormatString.ISO8601) : null },
                { "Url", Faker.Internet.Url() },
                { "Status", Faker.RandomNumber.Next(200, 600).ToString() },
                { "Stack", string.Join(Environment.NewLine,  Stack.Select(s => $"{s}{Environment.NewLine}{Environment.NewLine}").ToList()) }
            };

            Detail = new OltNgxLoggerDetailJson
            {
                Id = Faker.Lorem.GetFirstWord(),
                AppId = Result["AppId"],
                Message = Faker.Lorem.Sentence(),
                Name = Result["Name"],
                Status = Result["Status"],
                Time = UnixTime,
                Url = Result["Url"],
                User = Result["User"],
                Stack = Stack
            };
        }


        public long? UnixTime { get; }
        public DateTimeOffset? Timestamp { get; }
        public Dictionary<string, string> Result { get; }
        public List<OltNgxLoggerStackJson> Stack { get; }
        public OltNgxLoggerDetailJson Detail { get; }


        public OltNgxLoggerMessageJson BuildMessage(OltNgxLoggerLevel? level, OltNgxLoggerDetailJson detail)
        {
            var msg = new OltNgxLoggerMessageJson
            {
                Message = Faker.Lorem.Sentence(),
                Timestamp = Timestamp,
                FileName = Faker.Lorem.GetFirstWord(),
                LineNumber = Faker.RandomNumber.Next(1000, 4000).ToString(),
            };

            msg.Level = level ?? msg.Level;

            if (detail != null)
            {
                msg.Additional = new List<List<OltNgxLoggerDetailJson>>
                {
                    new List<OltNgxLoggerDetailJson>
                    {
                        detail
                    }
                };
            }

            Result.Add("Username", msg.GetUsername());
            Result.Add("Level", msg.Level?.ToString());
            Result.Add("LineNumber", msg.LineNumber);
            Result.Add("FileName", msg.FileName);
            Result.Add("Timestamp", msg.Timestamp?.ToString(OltSerilogConstants.FormatString.ISO8601));

            return msg;
        }

    }
}
