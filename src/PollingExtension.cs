using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TgBotFramework
{
    public static class PollingExtension
    {
        public static IBotFrameworkBuilder<TContext> UseLongPolling<TContext>(this IBotFrameworkBuilder<TContext> bot, ParallelMode mode = ParallelMode.SingleThreaded,
            LongPollingOptions longPollingOptions = null)
            where TContext : IUpdateContext
        {
            longPollingOptions ??= new LongPollingOptions();
            bot.ParallelMode = mode;
            return bot.UseLongPolling<PollingManager<TContext>>(longPollingOptions);
        }
    }
}