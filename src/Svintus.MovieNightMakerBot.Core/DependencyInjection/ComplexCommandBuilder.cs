using Microsoft.Extensions.DependencyInjection;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Svintus.MovieNightMakerBot.Core.DependencyInjection.Abstractions;

namespace Svintus.MovieNightMakerBot.Core.DependencyInjection;

internal sealed class ComplexCommandBuilder<TContext>(IServiceCollection serviceCollection) : IComplexCommandBuilder<TContext>
    where TContext : new()
{
    private readonly LinkedList<Type> _commandTypes = [];

    public IComplexCommandBuilder<TContext> WithStep<TCommand>() where TCommand : ComplexCommandBase<TContext>
    {
        if (_commandTypes.Contains(typeof(TCommand)))
        {
            throw new InvalidOperationException($"Command type {typeof(TCommand).Name} is already registered");
        }
        
        _commandTypes.AddFirst(typeof(TCommand));
        return this;
    }

    public IServiceCollection Build()
    {
        if (_commandTypes.Count == 0)
        {
            throw new InvalidOperationException("You must provide at least one command type");
        }

        foreach (var typeNode in _commandTypes.EnumerateNodes())
        {
            var type = typeNode.Value;
            var previousType = typeNode.Previous?.Value;

            if (typeNode.Next is null)
            {
                serviceCollection.AddSingleton<ICommand>(sp =>
                {
                    var parameters = previousType is not null ? new[] { sp.GetRequiredService(previousType) } : [];
                    return (ICommand)ActivatorUtilities.CreateInstance(sp, type, parameters);
                });

                break;
            }

            serviceCollection.AddSingleton(type, sp =>
            {
                var parameters = previousType is not null ? new[] { sp.GetRequiredService(previousType) } : [];
                return ActivatorUtilities.CreateInstance(sp, type, parameters);
            });
        }

        return serviceCollection;
    }
}

#region LinkedListExtensions

file static class LinkedListExtensions
{
    public static IEnumerable<LinkedListNode<T>> EnumerateNodes<T>(this LinkedList<T> list)
    {
        var node = list.First;

        while (node is not null)
        {
            yield return node;
            node = node.Next;
        }
    }
}

#endregion