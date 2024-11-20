using System.Diagnostics.CodeAnalysis;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;

namespace Svintus.MovieNightMakerBot.Core.UpdateDistribution;

internal sealed class UpdateDistributor : IUpdateDistributor
{
    private readonly Dictionary<long, IUpdateListener> _listeners = new(); 
    
    public void RegisterListener(long chatId, IUpdateListener distributeTo)
    {
        _listeners[chatId] = distributeTo;
    }

    public void UnregisterListener(long chatId)
    {
        _listeners.Remove(chatId);
    }

    public bool TryGetListener(long chatId, [NotNullWhen(true)] out IUpdateListener? listener)
    {
        return _listeners.TryGetValue(chatId, out listener);
    }
}