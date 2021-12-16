using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parrots;
using TgBotFramework;

await Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    // add each used in pipeline handler
    services.AddSingleton<ConsoleEchoHandler>();

    services.AddBotService<ExampleContext>("<bot token>", builder => builder
        .AddTestingUpdateProducer(mode: ParallelMode.Smart)
        .SetPipeline(pipeBuilder => pipeBuilder.Use<ConsoleEchoHandler>()
        )
    );
}).RunConsoleAsync();



public class ExampleContext : BaseUpdateContext { }
public class ConsoleEchoHandler : IUpdateHandler<ExampleContext>
{
    public async Task HandleAsync(ExampleContext context, UpdateDelegate<ExampleContext> next, CancellationToken cancellationToken)
    {
        if(context.Update.Id >= 19_999)
            Console.WriteLine(context.Update.Id);
    }
}