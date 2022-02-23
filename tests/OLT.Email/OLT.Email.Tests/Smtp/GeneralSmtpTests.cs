using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.Tests.Smtp
{
    public  class GeneralSmtpTests
    {
        private readonly string Host = Faker.Internet.DomainName();

        [Fact]
        public void PortNumberTests()
        {
            

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

            var serverNoPort = new OltSmtpServer
            {
                Host = Host,
                Port = null,
                Credentials = null
            };

            using (var client = OltSmtpEmailExtensions.BuildOltEmailClient(serverNoPort, true, smtpEmail).CreateClient())
            {
                Assert.True(client.Port > 0);
            }

            short? portNumber = 0;
            serverNoPort.Port = portNumber;

            using (var client = OltSmtpEmailExtensions.BuildOltEmailClient(serverNoPort, true, smtpEmail).CreateClient())
            {
                Assert.NotEqual(portNumber.Value, client.Port);
            }

            portNumber = 2525;
            serverNoPort.Port = portNumber;

            using (var client = OltSmtpEmailExtensions.BuildOltEmailClient(serverNoPort, true, smtpEmail).CreateClient())
            {
                Assert.Equal(portNumber.Value, client.Port);
            }

        }

        [Fact]
        public void TestCalendar()
        {

            var smtpServer = new OltSmtpServer
            {
                Host = Host,
                Credentials = null
            };

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
            var args = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, true, smtpEmail).WithCalendarInvite(bytes);

            using (var msg = args.CreateMessage(args.BuildRecipients()))
            {
                Assert.NotEmpty(msg.Headers);
                Assert.NotEmpty(msg.AlternateViews);
                Assert.Equal("urn:content-classes:calendarmessage", msg.Headers["Content-class"]);
                AlternateView avCal = msg.AlternateViews[0];
                Assert.NotNull(avCal);
                Assert.Equal("text/calendar", avCal.ContentType.MediaType);
                Assert.Equal("invite.ics", avCal.ContentType.Name);
                Assert.Equal("REQUEST", avCal.ContentType.Parameters["method"]);
            }

            
        }
    }
}
