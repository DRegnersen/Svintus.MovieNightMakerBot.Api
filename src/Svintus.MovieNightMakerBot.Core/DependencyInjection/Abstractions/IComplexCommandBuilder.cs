using Microsoft.Extensions.DependencyInjection;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;

namespace Svintus.MovieNightMakerBot.Core.DependencyInjection.Abstractions;

public interface IComplexCommandBuilder<TContext> where TContext : new()
{
    IComplexCommandBuilder<TContext> WithStep<TCommand>() where TCommand : ComplexCommandBase<TContext>;

    IServiceCollection Build();
}