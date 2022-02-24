using FluentAssertions;
using OLT.Email.SendGrid.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.SendGrid.Tests
{
    public class SendGridMessageTests
    {

        [Fact]
        public void TestProdEnabledRecipients()
        {
            var template = FakeEmailTagTemplate.FakerData(7, 5);
            var config = SendGridHelper.FakerConfig(true, 0, 0);
            
            var toList = new List<IOltEmailAddress>() { template.Recipients.To[1], template.Recipients.To[2] };
            var ccList = new List<IOltEmailAddress>() { template.Recipients.CarbonCopy[0], template.Recipients.CarbonCopy[2] };


            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template);

            var msg = args.CreateMessage(args.BuildRecipients());
            var personalization = msg.Personalizations[0];
            var compareTo = personalization.Tos.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();
            var compareCc = personalization.Ccs.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();
            Assert.Null(personalization.Bccs);

            compareTo.Should().BeEquivalentTo(template.Recipients.To);
            compareCc.Should().BeEquivalentTo(template.Recipients.CarbonCopy);


            args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template)
                .WithWhitelist(SendGridHelper.BuildWhitelist(toList))
                .WithWhitelist(SendGridHelper.BuildWhitelist(ccList));

            msg = args.CreateMessage(args.BuildRecipients());
            personalization = msg.Personalizations[0];
            compareTo = personalization.Tos.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();
            compareCc = personalization.Ccs.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();
            Assert.Null(personalization.Bccs);

            compareTo.Should().BeEquivalentTo(template.Recipients.To);
            compareCc.Should().BeEquivalentTo(template.Recipients.CarbonCopy);


        }


        [Fact]
        public void TestProdDisabledRecipients()
        {
            var template = FakeEmailTagTemplate.FakerData(7, 5);
            var config = SendGridHelper.FakerConfig(false, 0, 0);

            var toList = new List<IOltEmailAddress>() { template.Recipients.To[1], template.Recipients.To[2] };
            var ccList = new List<IOltEmailAddress>() { template.Recipients.CarbonCopy[0], template.Recipients.CarbonCopy[2] };

            var args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template);

            var msg = args.CreateMessage(args.BuildRecipients());
            var personalization = msg.Personalizations[0];
            Assert.Null(personalization.Tos);
            Assert.Null(personalization.Ccs);
            Assert.Null(personalization.Bccs);

            args = OltEmailSendGridExtensions.BuildOltEmailClient(config, template)
                .WithWhitelist(SendGridHelper.BuildWhitelist(toList))
                .WithWhitelist(SendGridHelper.BuildWhitelist(ccList));

            msg = args.CreateMessage(args.BuildRecipients());
            personalization = msg.Personalizations[0];


            var compareTo = personalization.Tos.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();
            var compareCc = personalization.Ccs.Select(x => new OltEmailAddress { Email = x.Email, Name = x.Name }).ToList();

            Assert.Null(personalization.Bccs);

            compareTo.Should().BeEquivalentTo(toList);
            compareCc.Should().BeEquivalentTo(ccList);

        }




    }
}
