using System.Diagnostics.CodeAnalysis;

namespace Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;

public interface IUpdateDistributor
{
    void RegisterListener(long chatId, IUpdateListener distributeTo);

    void UnregisterListener(long chatId);
    
    bool TryGetListener(long chatId, [NotNullWhen(true)] out IUpdateListener? listener); 
}