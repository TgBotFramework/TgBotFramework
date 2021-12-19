using Telegram.Bot.Types.Enums;
using TgBotFramework.WrapperExtensions;

namespace TgBotFramework
{
    public class In
    {
        public static bool PrivateChat(UpdateContext context) => 
            context.Update.GetChat()?.Type == ChatType.Private;
        
        public static bool Channel(UpdateContext context) => 
            context.Update.GetChat()?.Type == ChatType.Channel;
        
        public static bool GroupChat(UpdateContext context) => 
            context.Update.GetChat()?.Type == ChatType.Group;
        
        public static bool SupergroupChat(UpdateContext context) => 
            context.Update.GetChat()?.Type == ChatType.Supergroup;
        
        public static bool GroupOrSupergroup(UpdateContext context) => 
            context.Update.GetChat()?.Type is ChatType.Group or ChatType.Supergroup;
    }
}