using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.Commands.Abstractions;

public abstract class ComplexCommandBase<TContext>(IUpdateDistributor distributor) : CommandBase, IUpdateListener
    where TContext : new()
{
    private readonly Dictionary<long, int> _currentSteps = new();
    private readonly Dictionary<long, TContext> _contexts = new();
    
    public override async Task ExecuteAsync(Update update)
    {
        var chatId = update.Message!.Chat.Id;
        
        _currentSteps[chatId] = 1;
        _contexts[chatId] = new TContext();
        
        distributor.RegisterListener(chatId, this);

        await ExecuteAndContinueAsync(update);
    }

    public async Task HandleUpdateAsync(Update update)
    {
        if (update.Message?.Text is null)
            return;
        
        await ExecuteAndContinueAsync(update);
    }

    protected IReadOnlyDictionary<long, int> CurrentStep => _currentSteps.AsReadOnly();

    protected IReadOnlyDictionary<long, TContext> Context => _contexts.AsReadOnly();

    protected abstract Task<CommandStatus> ExecuteCoreAsync(Update update);

    private async Task ExecuteAndContinueAsync(Update update)
    {
        var status = await ExecuteCoreAsync(update);
        var chatId = update.Message!.Chat.Id;
        
        if (status == CommandStatus.Continue)
        {
            _currentSteps[chatId]++;
            return;
        }
        
        if (status == CommandStatus.Stop){
            distributor.UnregisterListener(update.Message!.Chat.Id);
            return;
        }

        throw new ArgumentOutOfRangeException(nameof(status));
    }
}

public enum CommandStatus
{
    Continue,
    Stop
}