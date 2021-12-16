using Microsoft.Extensions.DependencyInjection;

namespace TgBotFramework
{
    public class BotFrameworkBuilder<TContext> : BotFrameworkBuilder<TContext, BaseBot> where TContext : class, IUpdateContext
    {
        public BotFrameworkBuilder(IServiceCollection services) : base(services)
        {
        }
    }
}