using Microsoft.Extensions.DependencyInjection;
using TgBotFramework.UpdatePipeline;
using Xunit;

namespace TgBotFramework.Tests.Pipeline
{
    public class MapWhenT
    {
        private readonly ServiceCollection _services;

        public MapWhenT()
        {
            _services = new ServiceCollection();
            _services.AddScoped<Handlers.AHandler>();
            _services.AddScoped<Handlers.BHandler>();
            _services.AddScoped<Handlers.CHandler>();
        }

        [Theory(DisplayName = "Should call only selected handler")]
        [InlineData(true, false, "A")]
        [InlineData(true, true, "A")]
        [InlineData(false, false, "C")]
        [InlineData(false, true, "B")]
        public void Method_UseT_Should_Work(bool aValue, bool bValue, string results)
        {
            
            var provider = _services.BuildServiceProvider();
            var pipe = new BotPipelineBuilder<PipelineTestContext>( _services);

            var handler = pipe
                .MapWhen<Handlers.AHandler>(x => aValue)
                .MapWhen<Handlers.BHandler>(x => bValue)
                .MapWhen<Handlers.CHandler>(x => true)
                
                .Build();

            var context = new PipelineTestContext()
            {
                Update = new Telegram.Bot.Types.Update(),
                TestString = "",
                Services = provider
            };

            handler(context);

            Assert.Equal(results, context.TestString);
        }
    }
}