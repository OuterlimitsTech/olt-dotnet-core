using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OLT.Email.Tests.Smtp.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.Tests.Smtp
{

    public class SendTests
    {
        private readonly OltSmtpServer _smtpTestServer;
        private readonly IConfiguration _configuration;

        public SendTests(
            IConfiguration configuration,
            IOptions<OltSmtpServer> options)
        {
            _configuration = configuration;
            _smtpTestServer = options.Value;
        }

        [Fact]
        public async Task SmtpEmailExtensionsExceptionsTest()
        {


            Assert.Throws<ArgumentNullException>(() => OltSmtpEmailExtensions.BuildOltEmailClient(_smtpTestServer, true, new OltSmtpEmail()).Send());
            await Assert.ThrowsAsync<ArgumentNullException>(() => OltSmtpEmailExtensions.BuildOltEmailClient(_smtpTestServer, true, new OltSmtpEmail()).SendAsync());

            var smtpEmail = new OltSmtpEmail
            {
                Subject = Faker.Lorem.GetFirstWord(),
                Body = Faker.Lorem.Paragraph(4),
                From = new OltEmailAddress
                {
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email()
                },
            };

            Assert.Throws<OltEmailValidationException>(() => OltSmtpEmailExtensions.BuildOltEmailClient(_smtpTestServer, true, smtpEmail).Send());
            await Assert.ThrowsAsync<OltEmailValidationException>(() => OltSmtpEmailExtensions.BuildOltEmailClient(_smtpTestServer, true, smtpEmail).SendAsync());

        }

        [Fact]
        public void ApplicationErrorEmailTests()
        {
            var exceptionMessage = Faker.Lorem.Paragraph(10);
            var appName = $"Unit Test of {nameof(ApplicationErrorEmailTests)}";
            var environment = Faker.Company.Name();
            var email = Faker.Internet.Email();
            var ex = new Exception(exceptionMessage);


            var smtpEmail = new OltApplicationErrorEmail
            {
                Subject = Faker.Lorem.Words(34).Last(),
                Body = Faker.Lorem.Paragraph(4),
                AppName = appName,
                Environment = environment,
                From = new OltEmailAddress
                {
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email()
                },
                Recipients = new OltEmailRecipients
                {
                    To = new List<IOltEmailAddress>
                    {
                        new OltEmailAddress(email)
                    }
                }
            };

            Assert.Equal(appName, smtpEmail.AppName);
            Assert.Equal(environment, smtpEmail.Environment);

            var args = OltSmtpEmailExtensions.BuildOltEmailClient(ex, _smtpTestServer, smtpEmail);
            Assert.True(args.AllowSend(email));

            Assert.Throws<Exception>(() => OltSmtpEmailExtensions.OltEmailError(ex, _smtpTestServer, smtpEmail, true));

            try
            {
                OltSmtpEmailExtensions.OltEmailError(ex, _smtpTestServer, smtpEmail, false);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
            
            

        }

        [Fact]
        public async Task SmtpSendException()
        {
            var invalidServer = new OltSmtpServer
            {
                Host = _smtpTestServer.Host,
                Port = null,
                Credentials = null
            };

            var smtpEmail = new OltSmtpEmail
            {
                Subject = $"{Faker.Lorem.Words(10).Last()} {Faker.Lorem.Words(40).Last()}",
                Body = Faker.Lorem.Paragraph(4),
                From = new OltEmailAddress
                {
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email()
                },
                Recipients = new OltEmailRecipients
                {
                    To = new List<IOltEmailAddress>
                        {
                            new OltEmailAddress
                            {
                                Name = Faker.Name.FullName(),
                                Email = Faker.Internet.FreeEmail()
                            }
                        },
                },
            };

            var result1 = OltSmtpEmailExtensions.BuildOltEmailClient(invalidServer, true, smtpEmail).Send();
            var result2 = await OltSmtpEmailExtensions.BuildOltEmailClient(invalidServer, true, smtpEmail).SendAsync();

            Assert.False(result2.Success);
            result2.Should().BeEquivalentTo(result1);

        }

        [Fact]
        public async Task PortTests()
        {
            short? portNumber = 0;
            var subject = $"{Faker.Lorem.Words(10).Last()} {Faker.Lorem.Words(40).Last()}";

            var smtpEmail = new OltSmtpEmail
            {
                Subject = $"Port {portNumber} Test - {subject}",
                Body = Faker.Lorem.Paragraph(4),
                From = new OltEmailAddress
                {
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email()
                },
                Recipients = new OltEmailRecipients
                {
                    To = new List<IOltEmailAddress>
                        {
                            new OltEmailAddress
                            {
                                Name = Faker.Name.FullName(),
                                Email = Faker.Internet.FreeEmail()
                            }
                        },
                },
            };

            var validServerNoPort = new OltSmtpServer
            {
                Host = _smtpTestServer.Host,
                Port = portNumber,
                Credentials = new OltSmtpCredentials
                {
                    Username = _smtpTestServer.Credentials.Username,
                    Password = _smtpTestServer.Credentials.Password
                }
            };

            var result = await OltSmtpEmailExtensions.BuildOltEmailClient(validServerNoPort, true, smtpEmail).SendAsync();
            Assert.True(result.Success);

            portNumber = 2525;

            smtpEmail.Subject = $"Port {portNumber} Test - {subject}";

            validServerNoPort = new OltSmtpServer
            {
                Host = _smtpTestServer.Host,
                Port = portNumber,
                Credentials = new OltSmtpCredentials
                {
                    Username = _smtpTestServer.Credentials.Username,
                    Password = _smtpTestServer.Credentials.Password
                }
            };

            result = await OltSmtpEmailExtensions.BuildOltEmailClient(validServerNoPort, true, smtpEmail).SendAsync();
            Assert.True(result.Success);

        }

        [Fact]
        public async Task SmtpEmail()
        {
            Assert.NotNull(_smtpTestServer);
            Assert.NotNull(_smtpTestServer.Host);
            Assert.True(_smtpTestServer.Port > 0);
            Assert.NotNull(_smtpTestServer.Credentials.Username);
            Assert.NotNull(_smtpTestServer.Credentials.Password);
            Assert.False(_smtpTestServer.DisableSsl);

            var buildVersion = _configuration.GetValue<string>("GITHUB_RUN_NUMBER") ??
                               Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER") ??
                               "[No Run Number]";

            var now = DateTimeOffset.Now;
            var name = Faker.Name.FullName();
            var email = _configuration.GetValue<string>("SMTP_TO_ADDRESS") ?? Environment.GetEnvironmentVariable("SMTP_TO_ADDRESS");
            var bccName = Faker.Name.FullName();
            var bccEmail = Faker.Internet.Email();
            var bccName2 = Faker.Name.FullName();
            var bccEmail2 = Faker.Internet.Email();

            var fromName = $"{Faker.Name.First()} Unit Test";
            var fromEmail = _configuration.GetValue<string>("SMTP_FROM_ADDRESS") ?? Environment.GetEnvironmentVariable("SMTP_FROM_ADDRESS");
            var subject = $"Unit Test Email {now:f} Run - {buildVersion} ";
            var message = $"{buildVersion} -> This was generated on {now:f} from {this.GetType().Assembly.GetName().Name}.{nameof(SendTests)}.{nameof(SmtpEmail)}";


            var smtpEmail = new OltSmtpEmail
            {
                Subject = subject,
                Body = message,
                Recipients = new OltEmailRecipients
                {
                    To = new List<IOltEmailAddress>
                        {
                            new OltEmailAddress
                            {
                                Name = name,
                                Email = email
                            }
                        },
                    CarbonCopy = new List<IOltEmailAddress>
                        {
                            new OltEmailAddress
                            {
                                Name = bccName,
                                Email = bccEmail
                            },
                            new OltEmailAddress
                            {
                                Name = bccName2,
                                Email = bccEmail2
                            }
                        },
                },
                From = new OltEmailAddress
                {
                    Name = fromName,
                    Email = fromEmail
                },
            };


            Assert.NotNull(smtpEmail.Recipients);
            Assert.NotEmpty(smtpEmail.Recipients.To);
            Assert.NotEmpty(smtpEmail.Recipients.CarbonCopy);
            Assert.Equal(name, smtpEmail.Recipients.To[0].Name);
            Assert.Equal(email, smtpEmail.Recipients.To[0].Email);

            Assert.Equal(bccName, smtpEmail.Recipients.CarbonCopy[0].Name);
            Assert.Equal(bccEmail, smtpEmail.Recipients.CarbonCopy[0].Email);

            Assert.Equal(bccName2, smtpEmail.Recipients.CarbonCopy[1].Name);
            Assert.Equal(bccEmail2, smtpEmail.Recipients.CarbonCopy[1].Email);
            
            Assert.Equal(subject, smtpEmail.Subject);
            Assert.Equal(message, smtpEmail.Body);
            Assert.Equal(fromName, smtpEmail.From.Name);
            Assert.Equal(fromEmail, smtpEmail.From.Email);

            var result = OltSmtpEmailExtensions.BuildOltEmailClient(_smtpTestServer, true, smtpEmail).Send();
            Assert.True(result.Success);

            var result2 = await OltSmtpEmailExtensions.BuildOltEmailClient(_smtpTestServer, true, smtpEmail).SendAsync();
            Assert.True(result.Success);
            result2.Should().BeEquivalentTo(result);
            result2.RecipientResults.To.Should().HaveSameCount(smtpEmail.Recipients.To);
            result2.RecipientResults.CarbonCopy.Should().HaveSameCount(smtpEmail.Recipients.CarbonCopy);
        }

        [Fact]
        public void TestCalendar()
        {
            
            var created = DateTimeOffset.UtcNow.ToString("s").Replace(":", string.Empty).Replace("-", string.Empty);
            var start = DateTimeOffset.UtcNow.AddHours(6).ToString("s").Replace(":", string.Empty).Replace("-", string.Empty);
            var end = DateTimeOffset.UtcNow.AddHours(7).ToString("s").Replace(":", string.Empty).Replace("-", string.Empty);

            var calString = @$"
                BEGIN:VCALENDAR
                METHOD:REQUEST
                PRODID:-//github.com/rianjs/ical.net//NONSGML ical.net 4.0//EN
                VERSION:2.0
                BEGIN:VEVENT
                ATTENDEE;CN={Faker.Name.FullName()};RSVP=TRUE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:mailto:{Faker.Internet.Email()}
                ATTENDEE;CN={Faker.Name.FullName()};RSVP=TRUE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:mailto:{Faker.Internet.FreeEmail()}
                CREATED:{created}Z
                DTEND:{end}Z
                DTSTAMP:{created}Z
                DTSTART:{start}Z
                LAST-MODIFIED:{created}Z
                LOCATION:In a van down by the river
                ORGANIZER;CN={Faker.Name.FullName()}:mailto:{Faker.Internet.Email()}
                SEQUENCE:0
                STATUS:CONFIRMED
                SUMMARY:This is a bogus invite
                TRANSP:OPAQUE
                UID:9e31362b-4d65-44bc-b8ad-a29c7b80f294
                END:VEVENT
                END:VCALENDAR
                ".RemoveDoubleSpaces();

            var smtpEmail = new OltSmtpEmail
            {
                Subject = $"Invite Test to {Faker.Address.City()}",
                Body = Faker.Lorem.Paragraph(4),
                From = new OltEmailAddress
                {
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email()
                },
                Recipients = new OltEmailRecipients
                {
                    To = new List<IOltEmailAddress>
                        {
                            new OltEmailAddress
                            {
                                Name = Faker.Name.FullName(),
                                Email = Faker.Internet.FreeEmail()
                            }
                        },
                },
            };

            var bytes = Encoding.ASCII.GetBytes(calString);

            Assert.NotEmpty(bytes);
            var result1 = OltSmtpEmailExtensions.BuildOltEmailClient(_smtpTestServer, true, smtpEmail).WithCalendarInvite(bytes).Send();                       
            Assert.True(result1.Success);
        }


    }
}