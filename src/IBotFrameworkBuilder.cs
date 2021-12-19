using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgBotFramework.UpdatePipeline;

namespace TgBotFramework
{
    public interface IBotFrameworkBuilder<TContext> where TContext : UpdateContext
    {
        IServiceCollection Services { get; }
        UpdateContext Context { get; set; }
        BotPipelineBuilder<TContext> Pipeline { get; set; }
        ParallelMode ParallelMode { get; set; }

        IBotFrameworkBuilder<TContext> UseMiddleware<TMiddleware>() where TMiddleware : IUpdateHandler<TContext>;

        IBotFrameworkBuilder<TContext> SetPipeline(
            Func<IBotPipelineBuilder<TContext>, IBotPipelineBuilder<TContext>> pipeBuilder);

        IBotFrameworkBuilder<TContext> UseLongPolling<T>(LongPollingOptions longPollingOptions = null)
            where T : BackgroundService, IPollingManager;
        
        IBotFrameworkBuilder<TContext> UseCommands();
        IBotFrameworkBuilder<TContext> UseCommands(Assembly getAssembly);

        BotFramework<TContext> Build();
    }
    
}