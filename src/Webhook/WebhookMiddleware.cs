using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace TgBotFramework.Webhook
{
    public class WebhookMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ChannelWriter<IUpdateContext> _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WebhookMiddleware> _logger;
        private readonly WebhookSettings _settings;

        public WebhookMiddleware(RequestDelegate next, 
            Channel<IUpdateContext> channel, IServiceProvider serviceProvider, ILogger<WebhookMiddleware> logger, WebhookSettings settings)
        {
            _next = next;
            _channel = channel.Writer;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _settings = settings;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != HttpMethods.Post)
            {
                // in default returns 404
                await _next.Invoke(context);
                return;
            }
            
            string body;
            using (var reader = new StreamReader(context.Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }
            
            Update update = null;
            try
            {
                update = JsonConvert.DeserializeObject<Update>(body);
            }
            catch (JsonException e)
            {
                _logger.LogError("Unable to deserialize update body. {0}", e.Message);
            }
            
            if (update == null)
            {
                // it is unlikely of Telegram to send an invalid update object.
                // respond with "404 Not Found" in case an attacker is trying to find the webhook URL
                context.Response.StatusCode = 404;
                return;
            }

            var ctx = _serviceProvider.GetService<IUpdateContext>();
            ctx.Update = update;
            await _channel.WriteAsync(ctx);
            if (_settings.WaitForResult)
            {
                ctx.HttpContext = context;
                ctx.Result = new TaskCompletionSource();
                //await ctx.Result.Task;
                await Task.WhenAny(ctx.Result.Task, Task.Delay(2000));
            }
        }
    }
}