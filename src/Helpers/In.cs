using Telegram.Bot.Types.Enums;
using TgBotFramework.WrapperExtensions;

namespace TgBotFramework
{
    public class In
    {
        public static bool PrivateChat(IUpdateContext context) => 
            context.Update.GetChat()?.Type == ChatType.Private;
        
        public static bool Channel(IUpdateContext context) => 
            context.Update.GetChat()?.Type == ChatType.Channel;
        
        public static bool GroupChat(IUpdateContext context) => 
            context.Update.GetChat()?.Type == ChatType.Group;
        
        public static bool SupergroupChat(IUpdateContext context) => 
            context.Update.GetChat()?.Type == ChatType.Supergroup;
        
        public static bool GroupOrSupergroup(IUpdateContext context) => 
            context.Update.GetChat()?.Type is ChatType.Group or ChatType.Supergroup;
    }
}