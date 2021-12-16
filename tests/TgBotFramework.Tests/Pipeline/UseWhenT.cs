using Microsoft.Extensions.DependencyInjection;
using TgBotFramework.UpdatePipeline;
using Xunit;

namespace TgBotFramework.Tests.Pipeline
{
    public class UseWhenT
    {
        private readonly ServiceCollection _services;

        public UseWhenT()
        {
            _services = new ServiceCollection();
            _services.AddScoped<Handlers.AHandler>();
            _services.AddScoped<Handlers.BHandler>();
            _services.AddScoped<Handlers.CHandler>();
        }

        [Theory(DisplayName = "Should call only selected handler")]
        [InlineData(true, false, "AC")]
        [InlineData(true, true, "ABC")]
        [InlineData(false, false, "C")]
        [InlineData(false, true, "BC")]
        public void Method_UseT_Should_Work(bool aValue, bool bValue, string resuls)
        {

            var provider = _services.BuildServiceProvider();
            var pipe = new BotPipelineBuilder<PipelineTestContext>(_services);

            var handler = pipe
                .UseWhen<Handlers.AHandler>(x => aValue)
                .UseWhen<Handlers.BHandler>(x => bValue)
                .UseWhen<Handlers.CHandler>(x => true)
                .UseWhen<Handlers.CHandler>(x => false)

                .Build();

            var context = new PipelineTestContext()
            {
                Update = new Telegram.Bot.Types.Update(),
                TestString = "",
                Services = provider
            };

            handler(context);

            Assert.Equal(resuls, context.TestString);
        }
    }
}