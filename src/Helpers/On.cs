using System;
using Telegram.Bot.Types.Enums;

namespace TgBotFramework
{
    public class On
    {
        public static bool Message(IUpdateContext context) => context.Update.Type == UpdateType.Message;
        public static bool Poll(IUpdateContext context) => context.Update.Type == UpdateType.Poll;
        public static bool Unknown(IUpdateContext context) => context.Update.Type == UpdateType.Unknown;
        public static bool CallbackQuery(IUpdateContext context) => context.Update.Type == UpdateType.CallbackQuery;
        public static bool ChannelPost(IUpdateContext context) => context.Update.Type == UpdateType.ChannelPost;
        public static bool ChatMember(IUpdateContext context) => context.Update.Type == UpdateType.ChatMember;
        public static bool EditedMessage(IUpdateContext context) => context.Update.Type == UpdateType.EditedMessage;
        public static bool InlineQuery(IUpdateContext context) => context.Update.Type == UpdateType.InlineQuery;
        public static bool PollAnswer(IUpdateContext context) => context.Update.Type == UpdateType.PollAnswer;
        public static bool ShippingQuery(IUpdateContext context) => context.Update.Type == UpdateType.ShippingQuery;
        public static bool ChosenInlineResult(IUpdateContext context) => context.Update.Type == UpdateType.ChosenInlineResult;
        public static bool EditedChannelPost(IUpdateContext context) => context.Update.Type == UpdateType.EditedChannelPost;
        public static bool MyChatMember(IUpdateContext context) => context.Update.Type == UpdateType.MyChatMember;
        public static bool PreCheckoutQuery(IUpdateContext context) => context.Update.Type == UpdateType.PreCheckoutQuery;
    }
}