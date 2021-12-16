using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace TgBotFramework.Webhook
{
    public class WebhookSettings
    {
        /// <summary>
        /// Array of the update types you want your bot to receive.
        /// </summary>
        public UpdateType[] AllowedUpdates { get; set; }
        
        /// <summary>
        /// Will be implemented later
        /// </summary>
        public bool WaitForResult { get; set; } = false;
        
        /// <summary>
        /// Returns bot domain. Will be set in UseWebhook Method
        /// </summary>
        public string WebhookDomain { get; internal set; }
        /// <summary>
        /// Returns bot path. Will be set in UseWebhook Method
        /// </summary>
        public string WebhookPath { get; internal set; }

        /// <summary>
        /// Returns link to webhook.
        /// </summary>
        public string WebhookUrl => WebhookDomain + WebhookPath;

        /// <summary>
        /// Upload your public key certificate so that the root certificate in use can be checked. See our <see href="https://core.telegram.org/bots/self-signed">self-signed guide</see> for details
        /// </summary>
        public InputFileStream Certificate { get; set; }
        
        /// <summary>
        /// The fixed IP address which will be used to send webhook requests instead of the IP address resolved through DNS
        /// </summary>
        public string? IpAddress { get; set; }
        
        /// <summary>
        /// Maximum allowed number of simultaneous HTTPS connections to the webhook for update delivery, 1-100. Defaults to <i>40</i>. Use lower values to limit the load on your bot's server, and higher values to increase your bot's throughput
        /// </summary>
        public int? MaxConnection { get; set; }
        
        /// <summary>
        /// Pass True to drop all pending updates
        /// </summary>
        public bool? DropPendingUpdates { get; set; }
        
        /// <summary>
        /// Prints requests to current ILogger<>
        /// </summary>
        public bool DebugOutput { get; set; } = false;
    }
}