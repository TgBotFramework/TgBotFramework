using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgBotFramework.Attributes;
using TgBotFramework.UpdatePipeline;
using TgBotFramework.UpdateProcessing;

namespace TgBotFramework
{
    public class BotFrameworkBuilder<TContext, TBot> : IBotFrameworkBuilder<TContext>
        where TContext : UpdateContext
        where TBot : BaseBot
    {
        public IServiceCollection Services { get; set; }
        public UpdateContext Context { get; set; }
        public ParallelMode ParallelMode { get; set; }
        public BotPipelineBuilder<TContext> Pipeline { get; set; }
        

        public BotFrameworkBuilder(IServiceCollection services)
        {
            Services = services;
            Services.AddTransient<TContext>();
            Services.AddTransient<UpdateContext>(x => x.GetService<TContext>());
            Services.AddSingleton(Channel.CreateBounded<UpdateContext>(
                new BoundedChannelOptions(100)
                {
                    SingleReader = true
                })
            );
            
            services.AddSingleton<TBot>();
            if(typeof(TBot) != typeof(BaseBot))
                services.AddSingleton<BaseBot>(provider => provider.GetService<TBot>());

            Pipeline = new BotPipelineBuilder<TContext>(new ServiceCollection());
        }

        public IBotFrameworkBuilder<TContext> UseMiddleware<TMiddleware>() where TMiddleware : IUpdateHandler<TContext>
        {
            Pipeline.Use<TMiddleware>();

            return this;
        }
        public IBotFrameworkBuilder<TContext> SetPipeline(Func<IBotPipelineBuilder<TContext>, IBotPipelineBuilder<TContext>> pipeBuilder) 
        {
            pipeBuilder(Pipeline);
            return this;
        }

        public IBotFrameworkBuilder<TContext> UseLongPolling<T>(LongPollingOptions longPollingOptions = null) where T : BackgroundService, IPollingManager 
        {
            longPollingOptions ??= new LongPollingOptions();
            Services.AddHostedService<T>();
            Services.AddSingleton(longPollingOptions);
            Services.AddSingleton<IPollingManager>(x=>x.GetService<T>());
            return this;
        }
        
        public IBotFrameworkBuilder<TContext> UseCommands()
        {
            return UseCommands(Assembly.GetAssembly(typeof(TContext)));
        }

        public IBotFrameworkBuilder<TContext> UseCommands(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(x => 
                    x.GetCustomAttribute<CommandAttribute>()!=null && 
                    x.BaseType == typeof(CommandBase<TContext>) && 
                    x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IUpdateHandler<>)
                    ))
                .ToList();

            var sortedDictionary = new SortedDictionary<string, Type>();
            
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<CommandAttribute>();
                Debug.Assert(attribute != null, nameof(attribute) + " != null");
                sortedDictionary.Add(attribute.Text, type);
            }

            Pipeline.CheckCommands(sortedDictionary);
            
            return this;
        }

        public BotFramework<TContext> Build()
        {
            return new BotFramework<TContext>(Pipeline);
        }
    }
}