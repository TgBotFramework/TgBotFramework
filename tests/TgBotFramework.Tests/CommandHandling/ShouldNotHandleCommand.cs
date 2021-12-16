using System;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Xunit;

namespace TgBotFramework.Tests.CommandHandling
{
    public class ShouldNotHandleCommand
    {
        private BaseBot bot = new BaseBot("123123:AKSJDLKJALKSDJASKJDJLAA", "Test_Bot");
        
        [Theory(DisplayName = "Should reject parsing non-command text messages as command \"/test\"")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("/\t")]
        [InlineData("    ")]
        [InlineData("I AM NOT A COMMAND")]
        [InlineData("/testt")]
        [InlineData("/@test_bot")]
        [InlineData("/tes@test_bot")]
        [InlineData("not starts with command /tes@test_bot")]
        public void Should_Not_Parse_Invalid_Command_Text(string text)
        {
            Message message = new Message
            {
                Text = text,
                Entities = Array.Empty<MessageEntity>()
            };

            bool result = bot.CanHandleCommand("test", message);

            Assert.False(result);
        }

        [Theory(DisplayName = "Should reject parsing non-command caption as command \"/test\"")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("/\t")]
        [InlineData("    ")]
        [InlineData("I AM NOT A COMMAND")]
        [InlineData("/testt")]
        [InlineData("/@test_bot")]
        [InlineData("/tes@test_bot")]
        [InlineData("not starts with command /tes@test_bot")]
        public void Should_Not_Parse_Invalid_Caption(string text)
        {
            Message message = new Message
            {
                Caption = text,
                Entities = Array.Empty<MessageEntity>()
            };

            bool result = bot.CanHandleCommand("test", message);

            Assert.False(result);
        }
    }
}