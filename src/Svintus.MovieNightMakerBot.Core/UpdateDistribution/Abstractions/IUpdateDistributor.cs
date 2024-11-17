using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;

public interface IUpdateDistributor
{
    void DistributeTo(IUpdateListener listener, long chatId);

    void DistributeToDefault(long chatId);
    
    Task HandleUpdateAsync(Update update);
}