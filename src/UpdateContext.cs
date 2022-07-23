using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBotFramework.WrapperExtensions;

namespace TgBotFramework
{
    public class UpdateContext
    {
        public Update Update { get; set; }
        public HttpContext HttpContext { get; set; }
        public IServiceProvider Services { get; set; }
        public TaskCompletionSource Result { get; set; }
        public BaseBot Bot { get; set; }
        public TelegramBotClient Client { get; set; }

        
        
        private Chat _chat;
        public Chat Chat => _chat ??= Update.GetChat();
        public long? ChatId => Chat?.Id;

        
        private User _sender;
        public User Sender => _sender ??= Update.GetSender();
        public long? SenderId => Sender?.Id;
    }
}