using System.Threading;
using System.Threading.Tasks;

namespace TgBotFramework
{
    public delegate Task UpdateDelegate<in TContext>(TContext context, CancellationToken cancellationToken = default) where TContext : IUpdateContext;
}