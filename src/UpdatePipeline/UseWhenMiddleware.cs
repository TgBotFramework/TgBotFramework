using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TgBotFramework.UpdatePipeline
{

    public class UseWhenMiddleware<TContext> : IUpdateHandler<TContext> where TContext : UpdateContext
    {
        private readonly Predicate<TContext> _predicate;

        private readonly UpdateDelegate<TContext> _branch;

        public UseWhenMiddleware(Predicate<TContext> predicate, UpdateDelegate<TContext> branch)
        {
            _predicate = predicate;
            _branch = branch;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task HandleAsync(TContext context, UpdateDelegate<TContext> next, CancellationToken cancellationToken)
        {
            if (_predicate(context))
            {
                await _branch(context, cancellationToken);
            }

            await next(context, cancellationToken);
        }
    }
}