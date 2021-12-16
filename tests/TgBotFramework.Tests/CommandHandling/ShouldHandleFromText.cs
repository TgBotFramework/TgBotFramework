using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace TgBotFramework.Tests.CommandHandling
{
    public class ShouldHandleFromText
    {
        private BaseBot bot = new BaseBot("123123:AKSJDLKJALKSDJASKJDJLAA", "Test_Bot");
        
        [Theory(DisplayName = "Should accept handling valid \"/test\" commands for bot \"@Test_bot\" in text")]
        [InlineData("/test", "/test")]
        [InlineData("/test    ", "/test")]
        [InlineData("/test abc", "/test")]
        [InlineData("/TesT", "/tESt")]
        [InlineData("/test@test_bot", "/test@test_bot")]
        [InlineData("/test@test_bot ", "/test@test_bot")]
        [InlineData("/test@test_bot  !", "/test@test_bot")]
        public void Should_Parse_Valid_CommandsFromMessage(string text, string commandValue)
        {
            Message message = new Message
            {
                Text = text,
                Entities = new[]
                {
                    new MessageEntity
                    {
                        Type = MessageEntityType.BotCommand,
                        Offset = text.IndexOf(commandValue, StringComparison.OrdinalIgnoreCase),
                        Length = commandValue.Length
                    },
                },
            };

            bool result = bot.CanHandleCommand("test", message);

            Assert.True(result);
        }
    }
}