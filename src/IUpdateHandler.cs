using System.Threading;
using System.Threading.Tasks;

namespace TgBotFramework
{
    /// <summary>
    /// Part of the pipeline. Processes an update
    /// </summary>
    public interface IUpdateHandler<TContext> where TContext : IUpdateContext
    {
        Task HandleAsync(TContext context, UpdateDelegate<TContext> next, CancellationToken cancellationToken) ;
    }
}