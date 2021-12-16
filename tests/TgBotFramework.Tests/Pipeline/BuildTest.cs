using System;
using Microsoft.Extensions.DependencyInjection;
using TgBotFramework.UpdatePipeline;
using Xunit;

namespace TgBotFramework.Tests.Pipeline
{
    public  class BuildTests
    {
        readonly ServiceCollection _services = new();

        [Fact]
        public void ShouldThrowNull()
        {
            Assert.Throws<ArgumentNullException>(() => { new BotPipelineBuilder<PipelineTestContext>( null); });
        }

        [Fact]
        public void ShouldThrowError()
        {
            var pipe = new BotPipelineBuilder<PipelineTestContext>(_services);
            pipe.Use<Handlers.NullRefHandlerHandler>();
            pipe.Build();
            Assert.True(true);
        }
        
    }
}