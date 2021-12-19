using System.Diagnostics;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using TgBotFramework;

namespace Parrots;

public static class TestingExt
{
    public static IBotFrameworkBuilder<TContext> AddTestingUpdateProducer<TContext>(this IBotFrameworkBuilder<TContext> bot, ParallelMode mode) where TContext : UpdateContext
    {
        bot.ParallelMode = mode;
        bot.Services.AddHostedService<UpdateProducer<TContext>>();
        return bot;
    }
}

public class UpdateProducer<TContext> : BackgroundService, IPollingManager
    where TContext : UpdateContext
{
    private readonly ILogger<UpdateProducer<TContext>> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ChannelWriter<UpdateContext> _channel;
        
    public UpdateProducer(
        ILogger<UpdateProducer<TContext>> logger, 
        Channel<UpdateContext> channel,
        IServiceProvider serviceProvider) 
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _channel = channel.Writer;
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Started");
        Stopwatch sw = new Stopwatch();
        sw.Start();
        int counter = 0;
        while (counter < 20_000)
        {
            var context = _serviceProvider.GetService<TContext>();
            context!.Update = new Update() { Id = counter++ };
            await _channel.WriteAsync(context, stoppingToken);
        }
        sw.Stop();
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.LogInformation(sw.ElapsedMilliseconds.ToString());
    }
}