using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.Commands.Abstractions;

public abstract class ComplexCommandBase(IUpdateDistributor distributor, ComplexCommandBase? next = null)
    : CommandBase, IUpdateListener
{
    public override async Task ExecuteAsync(Update update)
    {
        await ExecuteAndConveyAsync(update);
    }

    public virtual async Task HandleUpdateAsync(Update update)
    {
        if (update.Message?.Text is null)
            return;
        
        await ExecuteAndConveyAsync(update);
    }
    
    protected abstract Task ExecuteCoreAsync(Update update);

    private async Task ExecuteAndConveyAsync(Update update)
    {
        await ExecuteCoreAsync(update);
        
        var chatId = update.Message!.Chat.Id;
        
        if (next is not null)
        {
            distributor.DistributeTo(next, chatId);
        }
        else
        {
            distributor.DistributeToDefault(chatId);
        }
    }
}