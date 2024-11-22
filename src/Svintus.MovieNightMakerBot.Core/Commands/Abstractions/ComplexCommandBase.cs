﻿using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.Commands.Abstractions;

/// <typeparam name="TContext">
/// The type of the context that command will keep while executing.
/// </typeparam>
public abstract class ComplexCommandBase<TContext>(IUpdateDistributor distributor) : CommandBase, IUpdateListener
    where TContext : new()
{
    private readonly Dictionary<long, CommandStep> _steps = new();
    private readonly Dictionary<long, TContext> _contexts = new();

    public override async Task ExecuteAsync(Update update)
    {
        var chatId = update.Message!.Chat.Id;

        _steps[chatId] = new CommandStep();
        _contexts[chatId] = new TContext();

        distributor.RegisterListener(chatId, distributeTo: this);

        await ExecuteAndContinueAsync(update);
    }

    public async Task HandleUpdateAsync(Update update)
    {
        if (update.Message?.Text is null)
            return;

        await ExecuteAndContinueAsync(update);
    }

    /// <summary>
    /// The current step of the command for the specific chat id.
    /// </summary>
    /// <remarks>
    /// Initially its value is 1.
    /// </remarks>
    protected IReadOnlyDictionary<long, CommandStep> Step => _steps.AsReadOnly();

    /// <summary>
    /// The current context of the command for the specific chat id.
    /// </summary>
    /// <remarks>
    /// Initially it is a new instance of <typeparamref name="TContext" />.
    /// </remarks>
    protected IReadOnlyDictionary<long, TContext> Context => _contexts.AsReadOnly();

    protected abstract Task<CommandStatus> ExecuteCoreAsync(Update update);

    private async Task ExecuteAndContinueAsync(Update update)
    {
        var status = await ExecuteCoreAsync(update);
        var chatId = update.Message!.Chat.Id;

        if (status == CommandStatus.Repeat)
            return;

        if (status == CommandStatus.Continue)
        {
            _steps[chatId].Advance();
            return;
        }

        if (status == CommandStatus.Stop)
        {
            distributor.UnregisterListener(update.Message!.Chat.Id);
            return;
        }

        throw new ArgumentOutOfRangeException(nameof(status), "Unexpected command status");
    }

    protected sealed class CommandStep
    {
        public int Value { get; private set; } = 1;

        public void Advance() => Value++;

        public bool IsInitial() => Value == 1;

        public bool IsAt(int step) => Value == step;

        public override bool Equals(object? obj)
        {
            return obj is CommandStep step && Value == step.Value;
        }

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        public override int GetHashCode() => Value.GetHashCode();

        #region Operators

        public static bool operator ==(CommandStep left, int right) => left.Value == right;

        public static bool operator !=(CommandStep left, int right) => !(left == right);

        public static bool operator <(CommandStep left, int right) => left.Value < right;

        public static bool operator >(CommandStep left, int right) => left.Value > right;

        public static bool operator <=(CommandStep left, int right) => left.Value <= right;

        public static bool operator >=(CommandStep left, int right) => left.Value >= right;

        #endregion
    }

    protected enum CommandStatus
    {
        /// <summary>
        /// The current step should be repeated.
        /// </summary>
        Repeat,

        /// <summary>
        /// The command execution should continue to the next step.
        /// </summary>
        Continue,

        /// <summary>
        /// The command execution should be stopped.
        /// </summary>
        Stop
    }
}