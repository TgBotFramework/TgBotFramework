using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgBotFramework.Tests.Pipeline
{
    public class PipelineTestContext : UpdateContext
    {
        public string TestString { get; set; }
        public Update Update { get; set; }
        public HttpContext HttpContext { get; set; }
        public IServiceProvider Services { get; set; }
        public TaskCompletionSource Result { get; set; }
        public BaseBot Bot { get; set; }
        public TelegramBotClient Client { get; set; }
    }
}