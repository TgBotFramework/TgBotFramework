using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TgBotFramework.UpdatePipeline
{
    public interface IBotPipelineBuilder<TContext> where TContext : IUpdateContext
    {
        ServiceCollection ServiceCollection { get; }
        ICollection<Func<UpdateDelegate<TContext>, UpdateDelegate<TContext>>> Components { get; set; }
        
        IBotPipelineBuilder<TContext> Use(Func<UpdateDelegate<TContext>, UpdateDelegate<TContext>> middleware);
        IBotPipelineBuilder<TContext> Use<THandler>() 
            where THandler : IUpdateHandler<TContext>;
        IBotPipelineBuilder<TContext> Use<THandler>(THandler handler) 
            where THandler : IUpdateHandler<TContext>;
        
        
        IBotPipelineBuilder<TContext> UseWhen<THandler>(Predicate<TContext> predicate)
            where THandler : IUpdateHandler<TContext>;
        IBotPipelineBuilder<TContext> UseWhen(Predicate<TContext> predicate,
            Action<IBotPipelineBuilder<TContext>> configure);
        

        IBotPipelineBuilder<TContext> MapWhen(Predicate<TContext> predicate,
            Action<IBotPipelineBuilder<TContext>> configure);
        IBotPipelineBuilder<TContext> MapWhen<THandler>(Predicate<TContext> predicate)
            where THandler : IUpdateHandler<TContext>;

        
        IBotPipelineBuilder<TContext> UseCommand<TCommand>(string command) 
            where TCommand : CommandBase<TContext>;


        UpdateDelegate<TContext> Build();
    }
}