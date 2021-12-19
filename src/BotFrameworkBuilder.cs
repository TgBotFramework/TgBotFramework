using Microsoft.Extensions.DependencyInjection;

namespace TgBotFramework
{
    public class BotFrameworkBuilder<TContext> : BotFrameworkBuilder<TContext, BaseBot> where TContext : UpdateContext
    {
        public BotFrameworkBuilder(IServiceCollection services) : base(services)
        {
        }
    }
}