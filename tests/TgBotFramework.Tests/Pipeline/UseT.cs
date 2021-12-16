using System;
using Microsoft.Extensions.DependencyInjection;
using TgBotFramework.UpdatePipeline;
using Xunit;

namespace TgBotFramework.Tests.Pipeline
{
     public class UseT
    {
        ServiceCollection _services = new(); 

        [Fact]
        public void ShouldThrowNull()
        {
            Assert.Throws<ArgumentNullException>(() => { new BotPipelineBuilder<PipelineTestContext>(null); });
        }

        [Fact]
        public void Method_UseT_Should_Work()
        {
            _services = new ServiceCollection();
            _services.AddScoped<Handlers.AHandler>();
            var provider = _services.BuildServiceProvider();
            var pipe = new BotPipelineBuilder<PipelineTestContext>(_services);

            var handler = pipe
                .Use<Handlers.AHandler>()
                .Use<Handlers.AHandler>()
                .Use<Handlers.AHandler>()
                .Build();

            var context = new PipelineTestContext()
            {
                Update = new Telegram.Bot.Types.Update(),
                TestString = "",
                Services = provider
            };

            handler(context);

            Assert.Equal("AAA", context.TestString);
        }

        [Fact]
        public void Method_UseT_Should_Work2()
        {
            _services = new ServiceCollection();
            _services.AddScoped<Handlers.AHandler>();
            _services.AddScoped<Handlers.BHandler>();    

            var provider = _services.BuildServiceProvider();
            var pipe = new BotPipelineBuilder<PipelineTestContext>(_services);

            var handler = pipe
                .Use<Handlers.AHandler>()
                .Use<Handlers.BHandler>()
                .Use<Handlers.AHandler>()
                .Build();

            var context = new PipelineTestContext()
            {
                Update = new Telegram.Bot.Types.Update(),
                TestString = "",
                Services = provider
            };

            handler(context);

            Assert.Equal("ABA", context.TestString);
        }

        [Fact]
        public void Method_UseT_Should_Work3()
        {
            _services = new ServiceCollection();
            _services.AddScoped<Handlers.AHandler>();
            _services.AddScoped<Handlers.BHandler>();

            var provider = _services.BuildServiceProvider();
            var pipe = new BotPipelineBuilder<PipelineTestContext>(_services);

            var handler = pipe
                .Use<Handlers.AHandler>()
                .Use<Handlers.AHandler>()
                .Use<Handlers.BHandler>()
                .Build();

            var context = new PipelineTestContext()
            {
                Update = new Telegram.Bot.Types.Update(),
                TestString = "",
                Services = provider
            };

            handler(context);

            Assert.Equal("AAB", context.TestString);
        }


    }
}