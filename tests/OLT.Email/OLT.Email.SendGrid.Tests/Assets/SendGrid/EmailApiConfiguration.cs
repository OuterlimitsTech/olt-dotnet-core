using Microsoft.Extensions.Options;
using OLT.Email;
using OLT.Email.SendGrid;
using System.Collections.Generic;

namespace OLT.Libraries.UnitTest.Assets.Email.SendGrid
{
    public class EmailApiConfiguration : OltEmailConfiguration
    {

        private readonly OltEmailConfigurationSendGrid _settings;

        public EmailApiConfiguration(IOptions<OltEmailConfigurationSendGrid> settings)
        {
            _settings = settings.Value;
            
        }

        public override OltEmailAddress From => new OltEmailAddress
        {
            Name = _settings.From.Name ?? "OLT",
            Email = _settings.From.Email ?? "noreply@outerlimitstech.com"
        };

        public override bool Production => _settings.Production;
        public override OltEmailConfigurationWhitelist TestWhitelist => _settings.TestWhitelist;
        public string ApiKey => _settings.ApiKey;
        public bool ClickTracking => true;
    }

}