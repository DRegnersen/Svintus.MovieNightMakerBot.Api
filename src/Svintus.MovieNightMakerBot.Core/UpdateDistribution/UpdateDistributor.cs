using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.UpdateDistribution;

public sealed class UpdateDistributor(IUpdateListener defaultListener) : IUpdateDistributor
{
    private readonly Dictionary<long, IUpdateListener> _actualListeners = new();

    public void DistributeTo(IUpdateListener listener, long chatId)
    {
        _actualListeners[chatId] = listener;
    }

    public void DistributeToDefault(long chatId)
    {
        _actualListeners[chatId] = defaultListener;
    }
    
    public async Task HandleUpdateAsync(Update update)
    {
        if (update.Message is null)
            return;
        
        var chatId = update.Message.Chat.Id;

        if (!_actualListeners.TryGetValue(chatId, out var actualListener))
        {
            actualListener = defaultListener;
            _actualListeners.Add(chatId, actualListener);
        }
        
        await actualListener.HandleUpdateAsync(update);
    }
}