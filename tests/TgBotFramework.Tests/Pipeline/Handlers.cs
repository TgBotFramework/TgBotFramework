using System.Threading;
using System.Threading.Tasks;

namespace TgBotFramework.Tests.Pipeline
{
    public class Handlers
    {
        public class AHandler : IUpdateHandler<PipelineTestContext> 
        {
            public async Task HandleAsync(PipelineTestContext context, UpdateDelegate<PipelineTestContext> next, CancellationToken cancellationToken)
            {
                context.TestString += "A";
                await next(context, cancellationToken);
            }
        }
        public class BHandler : IUpdateHandler<PipelineTestContext>
        {
            public async Task HandleAsync(PipelineTestContext context, UpdateDelegate<PipelineTestContext> next, CancellationToken cancellationToken)
            {
                context.TestString += "B";
                await next(context, cancellationToken);
            }
        }
        public class CHandler : IUpdateHandler<PipelineTestContext>
        {
            public async Task HandleAsync(PipelineTestContext context, UpdateDelegate<PipelineTestContext> next, CancellationToken cancellationToken)
            {
                context.TestString += "C";
                await next(context, cancellationToken);
            }
        }

        public class NullRefHandlerHandler : IUpdateHandler<PipelineTestContext>
        {
            public async Task HandleAsync(PipelineTestContext context, UpdateDelegate<PipelineTestContext> next, CancellationToken cancellationToken)
            {
                throw new System.Exception();
            }
        }
    }
}