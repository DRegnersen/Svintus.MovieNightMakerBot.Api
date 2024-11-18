using Microsoft.Extensions.DependencyInjection;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions.Generic;

namespace Svintus.MovieNightMakerBot.Core.DependencyInjection.Generic;

public class ComplexCommandBuilder<TContext>(
    IServiceCollection serviceCollection,
    Func<ComplexCommandBase<TContext>?, IServiceProvider, ComplexCommandBase<TContext>> complexCommandProvider
)
    where TContext : new()
{
    private Func<ComplexCommandBase<TContext>?, IServiceProvider, ComplexCommandBase<TContext>> _finalProvider = complexCommandProvider;

    public ComplexCommandBuilder<TContext> WithNext<TCommand>(Func<ComplexCommandBase<TContext>?, IServiceProvider, TCommand> complexCommandProvider)
        where TCommand : ComplexCommandBase<TContext>
    {
        _finalProvider = (next, sp) => _finalProvider(complexCommandProvider(next, sp), sp);
        return this;
    }

    public IServiceCollection Build()
    {
        return serviceCollection.AddSingleton<ICommand>(sp => _finalProvider(null, sp));
    }
}