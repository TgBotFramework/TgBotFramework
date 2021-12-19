using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;
using TgBotFramework.WrapperExtensions;

namespace TgBotFramework.UpdateProcessing
{
    public class SmartProcessing<TBot, TContext> : BackgroundService
        where TBot : BaseBot
        where TContext : UpdateContext
    {
        private readonly ILogger<SmartProcessing<TBot, TContext>> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TBot _bot;
        private readonly BotFramework<TContext> _framework;
        private readonly ChannelReader<UpdateContext> _updatesQueue;
        private readonly Channel<UpdateContext> PrivateChatUpdateReader = Channel.CreateBounded<UpdateContext>(50);
        private readonly Channel<UpdateContext> PublicChatUpdateReader = Channel.CreateBounded<UpdateContext>(50);
        private readonly Channel<UpdateContext> ChannelUpdateReader = Channel.CreateBounded<UpdateContext>(50);
        private readonly Channel<UpdateContext> NoChatUpdateReader = Channel.CreateBounded<UpdateContext>(50);
        private readonly Channel<UpdateContext> InlineQueryUpdateReader = Channel.CreateBounded<UpdateContext>(50);

        public SmartProcessing(ILogger<SmartProcessing<TBot, TContext>> logger,
            IServiceProvider serviceProvider,
            Channel<UpdateContext> updatesQueue,
            TBot bot,
            BotFramework<TContext> framework)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _bot = bot;
            _updatesQueue = updatesQueue.Reader;

            //check pipeline
            _framework = framework;
            _framework.Check(serviceProvider);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            var executors = new Task[]
            {
                StartChannel(ChannelUpdateReader, stoppingToken),
                StartChannel(InlineQueryUpdateReader, stoppingToken),
                StartChannel(PrivateChatUpdateReader, stoppingToken),
                StartChannel(PublicChatUpdateReader, stoppingToken),
                StartChannel(NoChatUpdateReader, stoppingToken)
            }; 
            
            _logger.LogInformation("SmartProcessor starts working");
            await foreach (var update in _updatesQueue.ReadAllAsync(stoppingToken))
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("SmartProcessor stops work");
                    return;
                }

                switch (update.Update.GetChat()?.Type)
                {
                    case ChatType.Private:
                        await PrivateChatUpdateReader.Writer.WriteAsync(update, stoppingToken);
                        break;
                    case ChatType.Channel:
                        await ChannelUpdateReader.Writer.WriteAsync(update, stoppingToken);
                        break;
                    case ChatType.Group:
                    case ChatType.Supergroup:
                        await PublicChatUpdateReader.Writer.WriteAsync(update, stoppingToken);
                        break;
                    case ChatType.Sender:
                        await InlineQueryUpdateReader.Writer.WriteAsync(update, stoppingToken);
                        break;
                    case null:
                        await NoChatUpdateReader.Writer.WriteAsync(update, stoppingToken);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private async Task StartChannel(Channel<UpdateContext> channel, CancellationToken stoppingToken)
        {
            await Task.Yield();
            await foreach (var update in channel.Reader.ReadAllAsync(stoppingToken))
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    return;
                }
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    update.Services = scope.ServiceProvider;
                    update.Client = _bot.Client;
                    update.Bot = _bot;
                    await _framework.Execute((TContext)update, stoppingToken);
                    if (update.Result != null)
                    {
                        Task.Run(() => update.Result.TrySetResult(), stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "Oops");
                }
            }
        }
    }
}