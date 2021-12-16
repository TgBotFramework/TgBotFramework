namespace TgBotFramework
{
    public interface IBotSettings
    {
        string ApiToken { get; set; }
        string WebhookDomain { get; set; }
        string WebhookPath { get; set; }
        public string BaseUrl { get; set; }
    }
}