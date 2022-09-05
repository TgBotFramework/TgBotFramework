using System;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBotFramework.UpdatePipeline;
using TgBotFramework.UpdateProcessing;

namespace TgBotFramework
{
    // ReSharper disable once InconsistentNaming
    public static class DIExtensions
    {
        /// <summary>
        /// Adds and configures telegram bot updates processing 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TBot"></typeparam>
        /// <typeparam name="TContext">Your context object</typeparam>
        /// <returns></returns>
        public static IServiceCollection AddBotService<TBot, TContext>
            (this IServiceCollection services, Action<IBotFrameworkBuilder<TContext>> configure) 
            where TBot : BaseBot
            where TContext : UpdateContext
        {
            var builder = new BotFrameworkBuilder<TContext, TBot>(services);
            configure(builder);
            
            switch (builder.ParallelMode)
            {
                case ParallelMode.SingleThreaded:
                    services.AddHostedService<SingleThreadProcessor<TBot, TContext>>();
                    break;
                case ParallelMode.MultiThreaded:
                    services.AddHostedService<MultiThreadProcessor<TBot, TContext>>();
                    break;
                case ParallelMode.Smart:
                    services.AddHostedService<SmartProcessing<TBot, TContext>>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ParallelMode), builder.ParallelMode, null);
            }
            
            services.AddSingleton(builder.Build());
            
            return services;
        }

        /// <summary>
        /// Adds and configures telegram bot updates processing 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="botToken"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TContext">Your context object</typeparam>
        /// <returns></returns>
        public static IServiceCollection AddBotService<TContext>
            (this IServiceCollection services, string botToken, Action<IBotFrameworkBuilder<TContext>> configure)
            where TContext : UpdateContext 
        {
            services.AddSingleton<IOptions<BotSettings>>(Options.Create(new BotSettings(){ ApiToken = botToken} ));

            return services.AddBotService<BaseBot, TContext>(configure);
        }
        
        /// <summary>
        /// Adds and configures telegram bot updates processing 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="botToken"></param>
        /// <param name="configure"></param>
        /// <typeparam name="TContext">Your context object</typeparam>
        /// <returns></returns>
        public static IServiceCollection AddBotService<TContext>
            (this IServiceCollection services, IOptions<BotSettings> settings, Action<IBotFrameworkBuilder<TContext>> configure)
            where TContext : UpdateContext 
        {
            services.AddSingleton<IOptions<BotSettings>>(settings);

            return services.AddBotService<BaseBot, TContext>(configure);
        }
    }
}