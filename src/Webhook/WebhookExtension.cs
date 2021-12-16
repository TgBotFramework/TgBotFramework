using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Telegram.Bot;
using TgBotFramework.Exceptions;

namespace TgBotFramework.Webhook
{
    public static class WebhookExtension
    {
        public static IBotFrameworkBuilder<TContext> UseWebhook<TContext>(this IBotFrameworkBuilder<TContext> builder,
            WebhookSettings webhookSettings) where TContext : IUpdateContext
        {
            builder.Services.AddSingleton<WebhookSettings>(webhookSettings);

            return builder;
        }

        public static IApplicationBuilder UseTelegramBotWebhook(
            this IApplicationBuilder app
        )
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<BotSettings>>() ??
                          throw new FrameworkException("There is no settings");
            var settings = app.ApplicationServices.GetService<WebhookSettings>() ??
                           throw new FrameworkException("You should add .UseWebhook method during framework creation");
            settings.WebhookDomain = options.Value.WebhookDomain;
            settings.WebhookPath = options.Value.WebhookPath;

            if (settings.WebhookDomain == null || settings.WebhookPath == null)
            {
                throw new FrameworkException("Settings file should contain WebhookDomain AND WebhookPath");
            }
            
            app.Map(
                options.Value.WebhookPath,
                builder =>
                    builder.UseMiddleware<WebhookMiddleware>()
            );
            var bot = app.ApplicationServices.GetService<BaseBot>();

            bot.Client.SetWebhookAsync(settings.WebhookUrl, settings.Certificate, settings.IpAddress,
                settings.MaxConnection, settings.AllowedUpdates, settings.DropPendingUpdates).GetAwaiter().GetResult();

            var webhookInfo = bot.Client.GetWebhookInfoAsync().GetAwaiter().GetResult();

            app.ApplicationServices.GetService<ILogger<WebhookMiddleware>>().LogInformation("Webhook set, info:\n{0}",
                JsonConvert.SerializeObject(webhookInfo, Formatting.Indented));
            
            return app;
        }
        
        public static IApplicationBuilder UseTelegramBotWebhook<WebhookMiddleware>(
            this IApplicationBuilder app
        )
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<BotSettings>>() ??
                          throw new FrameworkException("There is no settings");
            var settings = app.ApplicationServices.GetService<WebhookSettings>() ??
                           throw new FrameworkException("You should add .UseWebhook method during framework creation");
            settings.WebhookDomain = options.Value.WebhookDomain;
            settings.WebhookPath = options.Value.WebhookPath;

            if (settings.WebhookDomain == null || settings.WebhookPath == null)
            {
                throw new FrameworkException("Settings file should contain WebhookDomain AND WebhookPath");
            }
            
            app.Map(
                options.Value.WebhookPath,
                builder =>
                    builder.UseMiddleware<WebhookMiddleware>()
            );
            var bot = app.ApplicationServices.GetService<BaseBot>();

            bot.Client.SetWebhookAsync(settings.WebhookUrl, settings.Certificate, settings.IpAddress,
                settings.MaxConnection, settings.AllowedUpdates, settings.DropPendingUpdates).GetAwaiter().GetResult();
            
            return app;
        }
    }
}