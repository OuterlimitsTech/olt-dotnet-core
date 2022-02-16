namespace OLT.Email
{
    public class OltEmailConfigurationSendGrid : OltEmailConfiguration
    {
        public virtual string ApiKey { get; set; }
        public virtual bool DisableClickTracking { get; set; }
    }
}