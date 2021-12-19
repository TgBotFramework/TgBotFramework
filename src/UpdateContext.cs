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

        
        private Chat _chat = null;
        public Chat Chat
        {
            get { return _chat ??= Update.GetChat(); }
        }
        public ChatId ChatId
        {
            get => Chat?.Id;
        }

        private User _sender = null;
        public User Sender
        {
            get => _sender ??= Update.GetSender();
        }

        public long? SenderId
        {
            get => Sender?.Id;
        }

    }
}