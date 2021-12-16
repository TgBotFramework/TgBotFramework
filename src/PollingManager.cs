using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using TgBotFramework.UpdatePipeline;

namespace TgBotFramework
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PollingManager<TContext> : BackgroundService, IPollingManager
        where TContext : IUpdateContext
    {
        private readonly ILogger<PollingManager<TContext>> _logger;
        
        private readonly LongPollingOptions _pollingOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly ChannelWriter<IUpdateContext> _channel;
        private readonly TelegramBotClient _client;
        
        public PollingManager(
            ILogger<PollingManager<TContext>> logger, 
            LongPollingOptions pollingOptions, 
            BaseBot bot,
            Channel<IUpdateContext> channel,
            IServiceProvider serviceProvider) 
        {
            _logger = logger;
            _pollingOptions = pollingOptions;
            _serviceProvider = serviceProvider;
            _channel = channel.Writer;
            _client = bot.Client;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await _client.DeleteWebhookAsync(cancellationToken: cancellationToken, dropPendingUpdates: _pollingOptions.DropPendingUpdates);
            
            if (_pollingOptions.DebugOutput)
            {
                _client.OnApiResponseReceived += ReceiveLogger;
            }

            int messageOffset = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Update[] updates = await _client.GetUpdatesAsync(messageOffset, 0, _pollingOptions.Timeout,
                        _pollingOptions.AllowedUpdates, cancellationToken: cancellationToken);

                    foreach (var update in updates)
                    {
                        var updateContext = _serviceProvider.GetService<IUpdateContext>();

                        Debug.Assert(updateContext != null, nameof(updateContext) + " != null");
                        updateContext.Update = update;

                        await _channel.WriteAsync(updateContext, cancellationToken);
                        messageOffset = update.Id + 1;

                        if (_pollingOptions.WaitForResult)
                        {
                            updateContext.Result = new TaskCompletionSource();
                            await updateContext.Result.Task;
                        }
                    }
                }
                catch (ApiRequestException e)
                {
                    _logger.LogError(e, "API Error in polling " + nameof(PollingManager<TContext>));
                }
                catch (RequestException e)
                {
                    _logger.LogError(e, "Network Error while polling in " + nameof(PollingManager<TContext>));
                }
                catch (TaskCanceledException e)
                {
                    _logger.LogInformation("Polling is shutting down...");
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "Error while polling in " + nameof(PollingManager<TContext>));
                }
            }
        }

        private async ValueTask ReceiveLogger(ITelegramBotClient client, ApiResponseEventArgs args, CancellationToken token)
        {
            if (args.ApiRequestEventArgs.MethodName == "getUpdates")
            {
                var message = await args.ResponseMessage.Content.ReadAsStringAsync(token);
                JToken jObj = JObject.Parse(message)["result"]; 
                foreach (var item in jObj)
                {
                    _logger.LogInformation("[{0}] Content:\n{1}", item["update_id"].ToString(), item);
                }
            }
        }
    }

    public interface IPollingManager { }
}