namespace OLT.Email.SendGrid
{
    public interface IOltEmailConfigurationSendGrid : IOltEmailConfiguration
    {
        string ApiKey { get; }
        bool DisableClickTracking { get; }
    }
}