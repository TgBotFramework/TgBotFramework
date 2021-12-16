using System;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TgBotFramework.WrapperExtensions
{
    public static class UpdateExtensions
    {
        /// <summary>
        /// Returns sender ID for updates with sender ID, 0 otherwise. Exception in case of new update type.   
        /// </summary>
        /// <param name="update"></param>
        /// <returns>Id or 0 (for updates that doesn`t have sender ID</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws exception if there is some new update.Type</exception>
        public static long GetSenderId(this Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Unknown:
                    return 0;
                case UpdateType.Message:
                    return update.Message.From.Id;
                case UpdateType.InlineQuery:
                    return update.InlineQuery.From.Id;
                case UpdateType.ChosenInlineResult:
                    return update.ChosenInlineResult.From.Id;
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery.From.Id;
                case UpdateType.EditedMessage:
                    return update.EditedMessage.From.Id;
                case UpdateType.ChannelPost:
                    return update.ChannelPost.From?.Id ?? 0;
                case UpdateType.EditedChannelPost:
                    return update.EditedChannelPost.From.Id;
                case UpdateType.ShippingQuery:
                    return update.ShippingQuery.From.Id;
                case UpdateType.PreCheckoutQuery:
                    return update.PreCheckoutQuery.From.Id;
                case UpdateType.Poll:
                    return 0;
                case UpdateType.PollAnswer:
                    return update.PollAnswer.User.Id;
                case UpdateType.ChatMember:
                    return update.ChatMember.From.Id;
                case UpdateType.MyChatMember:
                    return update.MyChatMember.From.Id;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Returns sender for updates with sender, null otherwise. Exception in case of new update type.   
        /// </summary>
        /// <param name="update"></param>
        /// <returns>User or null (for updates that doesn't have sender</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws exception if there is some new update.Type</exception>
        public static User GetSender(this Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Unknown:
                    return null;
                case UpdateType.Message:
                    return update.Message.From;
                case UpdateType.InlineQuery:
                    return update.InlineQuery.From;
                case UpdateType.ChosenInlineResult:
                    return update.ChosenInlineResult.From;
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery.From;
                case UpdateType.EditedMessage:
                    return update.EditedMessage.From;
                case UpdateType.ChannelPost:
                    return update.ChannelPost.From;
                case UpdateType.EditedChannelPost:
                    return update.EditedChannelPost.From;
                case UpdateType.ShippingQuery:
                    return update.ShippingQuery.From;
                case UpdateType.PreCheckoutQuery:
                    return update.PreCheckoutQuery.From;
                case UpdateType.Poll:
                    return null;
                case UpdateType.PollAnswer:
                    return update.PollAnswer.User;
                case UpdateType.ChatMember:
                    return update.ChatMember.From;
                case UpdateType.MyChatMember:
                    return update.MyChatMember.From;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Returns chat for updates with chat, null otherwise. Exception in case of new update type.   
        /// </summary>
        /// <param name="update"></param>
        /// <returns>Chat or null (for updates that doesn't have chat</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws exception if there is some new update.Type</exception>
        public static Chat GetChat(this Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    return update.Message.Chat;
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery.Message.Chat;
                case UpdateType.EditedMessage:
                    return update.EditedMessage.Chat;
                case UpdateType.ChannelPost:
                    return update.ChannelPost.Chat;
                case UpdateType.EditedChannelPost:
                    return update.EditedChannelPost.Chat;
                case UpdateType.ChatMember:
                    return update.ChatMember.Chat;
                case UpdateType.MyChatMember:
                    return update.MyChatMember.Chat;
                case UpdateType.Unknown:
                case UpdateType.InlineQuery:
                case UpdateType.ChosenInlineResult:
                case UpdateType.ShippingQuery:
                case UpdateType.PreCheckoutQuery:
                case UpdateType.Poll:
                case UpdateType.PollAnswer:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Just a simple way to display update
        /// </summary>
        /// <param name="update"></param>
        /// <param name="formatted"></param>
        /// <returns></returns>
        public static string ToJsonString(this Update update, bool formatted = true)
        {
            return JsonConvert.SerializeObject(update, formatted? Formatting.Indented : Formatting.None);
        }
    }
}