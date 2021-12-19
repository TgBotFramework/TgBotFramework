using System;
using Microsoft.Extensions.DependencyInjection;
using TgBotFramework.Exceptions;
using TgBotFramework.UpdatePipeline;

namespace TgBotFramework
{
    public class BotFramework<TContext> where TContext : UpdateContext
    {
        private readonly BotPipelineBuilder<TContext> _pipeline;
        private UpdateDelegate<TContext> _delegate;
        public UpdateDelegate<TContext> Execute => (_delegate ??= _pipeline.Build());

        internal BotFramework(BotPipelineBuilder<TContext> pipeline)
        {
            _pipeline = pipeline;
        }

        public void Check(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            foreach (var type in _pipeline.ServiceCollection)
            {
                Type typeToResolve = type.ImplementationType;
                if (type.ServiceType.IsGenericTypeDefinition)
                {
                    typeToResolve = type.ServiceType.MakeGenericType(typeof(TContext));
                }
                
                if (scope.ServiceProvider.GetService(typeToResolve) == null)
                {
                    throw new PipelineException(
                        $"There is no service type of {typeToResolve.FullName} in DI");
                }
            }
        }
    }
}