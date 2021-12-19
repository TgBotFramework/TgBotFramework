using System;
using Telegram.Bot.Types.Enums;

namespace TgBotFramework
{
    public class On
    {
        public static bool Message(UpdateContext context) => context.Update.Type == UpdateType.Message;
        public static bool Poll(UpdateContext context) => context.Update.Type == UpdateType.Poll;
        public static bool Unknown(UpdateContext context) => context.Update.Type == UpdateType.Unknown;
        public static bool CallbackQuery(UpdateContext context) => context.Update.Type == UpdateType.CallbackQuery;
        public static bool ChannelPost(UpdateContext context) => context.Update.Type == UpdateType.ChannelPost;
        public static bool ChatMember(UpdateContext context) => context.Update.Type == UpdateType.ChatMember;
        public static bool EditedMessage(UpdateContext context) => context.Update.Type == UpdateType.EditedMessage;
        public static bool InlineQuery(UpdateContext context) => context.Update.Type == UpdateType.InlineQuery;
        public static bool PollAnswer(UpdateContext context) => context.Update.Type == UpdateType.PollAnswer;
        public static bool ShippingQuery(UpdateContext context) => context.Update.Type == UpdateType.ShippingQuery;
        public static bool ChosenInlineResult(UpdateContext context) => context.Update.Type == UpdateType.ChosenInlineResult;
        public static bool EditedChannelPost(UpdateContext context) => context.Update.Type == UpdateType.EditedChannelPost;
        public static bool MyChatMember(UpdateContext context) => context.Update.Type == UpdateType.MyChatMember;
        public static bool PreCheckoutQuery(UpdateContext context) => context.Update.Type == UpdateType.PreCheckoutQuery;
    }
}