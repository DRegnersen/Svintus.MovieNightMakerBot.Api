using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.Commands.Abstractions.Generic;

public abstract class ComplexCommandBase<TContext>(IUpdateDistributor distributor, ComplexCommandBase<TContext>? next = null)
    : ComplexCommandBase(distributor, next) where TContext : new()
{
    public override async Task ExecuteAsync(Update update)
    {
        await base.ExecuteAsync(update);
        if (next is not null)
        {
            next.Context = Context;
        }
    }

    public override async Task HandleUpdateAsync(Update update)
    {
        await base.HandleUpdateAsync(update);
        if (next is not null)
        {
            next.Context = Context;
        }
    }
    
    protected TContext Context { get; set; } = new();
}