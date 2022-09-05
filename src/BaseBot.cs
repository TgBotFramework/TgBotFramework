using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = Telegram.Bot.Types.File;

namespace TgBotFramework
{
    public class BaseBot 
    {
        public TelegramBotClient Client { get; }
        
        /// <summary>
        /// Long running client for file downloads. (default client with timeout 10 min.) 
        /// </summary>
        public TelegramBotClient ClientForHugeFiles
        {
            get => Token!=null? new TelegramBotClient(Token) { Timeout = TimeSpan.FromMinutes(10) } : throw new NullReferenceException("ClientForHugeFiles needs bot token");
        }
        public string Username { get; }
        internal readonly string Token;

        public BaseBot(IOptions<BotSettings> options)
        {
            Token = options.Value.ApiToken;
            Client = new TelegramBotClient(options.Value.ApiToken); //fix base address adding
            Username = options.Value.Username ?? Client.GetMeAsync().GetAwaiter().GetResult().Username;
        }

        public BaseBot(string token, string username = null)
        {
            Client = new TelegramBotClient(token);
            Token = token;
            Username = username ?? Client.GetMeAsync().GetAwaiter().GetResult().Username;
        }

        public BaseBot(TelegramBotClient client)
        {
            Client = client;
            Username = Client.GetMeAsync().GetAwaiter().GetResult().Username;
        }

        public bool CanHandleCommand(string commandName, Message message)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentException("Invalid command name", nameof(commandName));
            if (commandName.StartsWith("/"))
                throw new ArgumentException("Command name must not start with '/'.", nameof(commandName));

            if (message == null)
                return false;

            if (message.Text != null && message.Entities is { Length: > 0 })
                return message.Entities[0].Type == MessageEntityType.BotCommand && message.Entities[0].Offset == 0 && Regex.IsMatch(
                    message.Text.Substring(message.Entities[0].Offset, message.Entities[0].Length),
                    $@"^/{commandName}(?:@{Username})?$",
                    RegexOptions.IgnoreCase);
            
            if (message.Caption != null && message.CaptionEntities is { Length: > 0 })
                return message.CaptionEntities[0].Type == MessageEntityType.BotCommand && message.CaptionEntities[0].Offset == 0 &&
                       Regex.IsMatch(
                        message.Caption.Substring(message.CaptionEntities[0].Offset, message.CaptionEntities[0].Length),
                        $@"^/{commandName}(?:@{Username})?$",
                        RegexOptions.IgnoreCase);

            return false;
        }
    }
}