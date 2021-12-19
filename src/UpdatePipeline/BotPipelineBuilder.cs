using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using TgBotFramework.DataStructures;
using TgBotFramework.Exceptions;

namespace TgBotFramework.UpdatePipeline
{
    public class BotPipelineBuilder<TContext> : IBotPipelineBuilder<TContext>
        where TContext : UpdateContext
    {
        public ServiceCollection ServiceCollection { get; }
        private UpdateDelegate<TContext> UpdateDelegate { get; set; }

        public ICollection<Func<UpdateDelegate<TContext>, UpdateDelegate<TContext>>> Components { get; set; }

        public BotPipelineBuilder(ServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            
            Components = new List<Func<UpdateDelegate<TContext>, UpdateDelegate<TContext>>>();
        }

        public UpdateDelegate<TContext> Build()
        {
            UpdateDelegate<TContext> handle = (context, cancellationToken) =>
            {
                return Task.FromResult(1);
            };

            foreach (var component in Components.Reverse())
            {
                handle = component(handle);
            }

            return UpdateDelegate = handle;
        }

        public IBotPipelineBuilder<TContext> Use(Func<UpdateDelegate<TContext>, UpdateDelegate<TContext>> middleware)
        {
            if (middleware == null) throw new ArgumentNullException(nameof(middleware));
            
            Components.Add(middleware);
            return this;
        }

        public IBotPipelineBuilder<TContext> Use<THandler>()
            where THandler : IUpdateHandler<TContext>
        {
            ServiceCollection.TryAddScoped(typeof(THandler));
            Components.Add(
                next =>
                    (context, cancellationToken) =>
                    {
                        if (context.Services.GetService(typeof(THandler)) is IUpdateHandler<TContext> handler)
                            return handler.HandleAsync(context, next, cancellationToken);
                        else
                            throw new PipelineException(
                                $"Unable to resolve handler of type {typeof(THandler).FullName}"
                            );
                    }
            );

            return this;
        }

        internal IBotPipelineBuilder<TContext> Use(Type type)
        {
            ServiceCollection.TryAddScoped(type);
            if (type.GetInterfaces().All(x => x != typeof(IUpdateHandler<TContext>)))
            {
                throw new PipelineException($"Type {type} doesn't implement {typeof(IUpdateHandler<TContext>)}");
            }

            Components.Add(
                next =>
                    (context, cancellationToken) =>
                    {
                        if (context.Services.GetService(type) is IUpdateHandler<TContext> handler)
                            return handler.HandleAsync(context, next, cancellationToken);
                        else
                            throw new PipelineException(
                                $"Unable to resolve handler of type {type.FullName}"
                            );
                    }
            );

            return this;
        }

        
        
        internal IBotPipelineBuilder<TContext> CheckCommands(SortedDictionary<string, Type> commands)
        {
            foreach (KeyValuePair<string,Type> pair in commands)
            {
                ServiceCollection.AddScoped(pair.Value);
            }
            Components.Add(next => (context, cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(context.Update.Message?.Text) || !(context.Update.Message.Text.StartsWith('/') || context.Update.Message.Text.Length>1 ) )
                {
                    return next(context, cancellationToken); 
                }
                
                var type = commands.PrefixSearch(context.Update.Message.Text[1..]);
                if (type != null)
                {
                    var realType = type;
                    if (type.IsGenericTypeDefinition)
                    {
                        realType = type.MakeGenericType(typeof(TContext));
                    }

                    if (context.Services.GetService(realType) is IUpdateHandler<TContext> handler)
                        return handler.HandleAsync(context, next, cancellationToken);
                    else
                    {
                        throw new PipelineException("Class wasn't registered: {0}", realType.FullName);
                    }
                }

                return next(context, cancellationToken);

            });

            return this;
        }
        

        public IBotPipelineBuilder<TContext> Use<THandler>(THandler handler)
            where THandler : IUpdateHandler<TContext>
        {
            Components.Add(next =>
                (context, cancellationToken) => handler.HandleAsync(context, next, cancellationToken)
            );

            return this;
        }

        public IBotPipelineBuilder<TContext> UseWhen(
            Predicate<TContext> predicate,
            Action<IBotPipelineBuilder<TContext>> configure)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (configure == null) throw new ArgumentNullException(nameof(configure));
            
            var branchBuilder = new BotPipelineBuilder<TContext>(ServiceCollection);
            configure(branchBuilder);
            UpdateDelegate<TContext> branchDelegate = branchBuilder.Build();

            Use(new UseWhenMiddleware<TContext>(predicate, branchDelegate));

            return this;
        }


        public IBotPipelineBuilder<TContext> UseWhen<THandler>(
            Predicate<TContext> predicate
        )
            where THandler : IUpdateHandler<TContext>
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            
            ServiceCollection.TryAddScoped(typeof(THandler));
            var branchDelegate = new BotPipelineBuilder<TContext>(ServiceCollection).Use<THandler>().Build();
            Use(new UseWhenMiddleware<TContext>(predicate, branchDelegate));

            return this;
        }

        public IBotPipelineBuilder<TContext> MapWhen(
            Predicate<TContext> predicate,
            Action<IBotPipelineBuilder<TContext>> configure)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (configure == null) throw new ArgumentNullException(nameof(configure));
            
            var mapBuilder = new BotPipelineBuilder<TContext>(ServiceCollection);
            configure(mapBuilder);
            var mapDelegate = mapBuilder.Build();

            Use(new MapWhenMiddleware<TContext>(predicate, mapDelegate));

            return this;
        }

        public IBotPipelineBuilder<TContext> MapWhen<THandler>(
            Predicate<TContext> predicate)
            where THandler : IUpdateHandler<TContext>
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            
            ServiceCollection.TryAddScoped(typeof(THandler));
            var branchDelegate = new BotPipelineBuilder<TContext>(ServiceCollection).Use<THandler>().Build();

            Use(new MapWhenMiddleware<TContext>(predicate, branchDelegate));

            return this;
        }

        public IBotPipelineBuilder<TContext> UseCommand<TCommand>(
            string command
        )
            where TCommand : CommandBase<TContext>
        {
            if (string.IsNullOrWhiteSpace(command) || command.StartsWith("/"))
                throw new ArgumentException("Command text shouldn't be null, empty or starts with /");
            
            return MapWhen(
                    ctx => ctx.Bot.CanHandleCommand(command, ctx.Update.Message),
                    botBuilder => botBuilder.Use<TCommand>()
                );
        }
    }
}
