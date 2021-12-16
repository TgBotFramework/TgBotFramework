namespace TgBotFramework
{
    public class BotSettings : IBotSettings
    {
        public string ApiToken { get; set; }
        public string WebhookDomain { get; set; }
        public string WebhookPath { get; set; }
        public string BaseUrl { get; set; }
        public string Username { get; set; }
    }
}