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
    }
}