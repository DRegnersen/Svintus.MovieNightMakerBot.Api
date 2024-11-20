using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.Commands.Abstractions;

public abstract class ComplexCommandBase<TContext>(IUpdateDistributor distributor, ComplexCommandBase<TContext>? next = null)
    : CommandBase, IUpdateListener where TContext : new()
{
    public override async Task ExecuteAsync(Update update)
    {
        await ExecuteAndConveyAsync(update);
    }

    public async Task HandleUpdateAsync(Update update)
    {
        if (update.Message?.Text is null)
            return;
        
        await ExecuteAndConveyAsync(update);
    }

    protected TContext Context { get; set; } = new();

    protected abstract Task ExecuteCoreAsync(Update update);

    private async Task ExecuteAndConveyAsync(Update update)
    {
        await ExecuteCoreAsync(update);

        var chatId = update.Message!.Chat.Id;

        if (next is not null)
        {
            distributor.RegisterListener(chatId, distributeTo: next);
            next.Context = Context;
        }
        else
        {
            distributor.UnregisterListener(chatId);
        }

        Context = new TContext();
    }
}