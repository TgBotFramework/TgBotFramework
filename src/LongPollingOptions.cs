using Telegram.Bot.Types.Enums;

namespace TgBotFramework
{
    public class LongPollingOptions
    {
        /// <summary>
        /// Timeout in seconds for long polling. Defaults to 0, i.e. usual short polling. Should be positive, short polling should be used for testing purposes only.
        /// </summary>
        public int Timeout { get; set; } = 60;
        
        /// <summary>
        /// Array of the update types you want your bot to receive.
        /// </summary>
        public UpdateType[] AllowedUpdates { get; set; }
        
        /// <summary>
        /// Will be implemented later
        /// </summary>
        public bool WaitForResult { get; set; } = false;

        /// <summary>
        /// Prints requests to current ILogger<>
        /// </summary>
        public bool DebugOutput { get; set; } = false;

        /// <summary>
        /// Drops pending updates
        /// </summary>
        public bool DropPendingUpdates { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public ParallelMode ParallelMode { get; set; } = ParallelMode.SingleThreaded;
    }
}