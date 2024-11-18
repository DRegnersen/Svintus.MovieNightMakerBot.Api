using Microsoft.Extensions.DependencyInjection;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;

namespace Svintus.MovieNightMakerBot.Core.DependencyInjection;

public class ComplexCommandBuilder(
    IServiceCollection serviceCollection,
    Func<ComplexCommandBase?, IServiceProvider, ComplexCommandBase> complexCommandProvider
)
{
    private Func<ComplexCommandBase?, IServiceProvider, ComplexCommandBase> _finalProvider = complexCommandProvider;

    public ComplexCommandBuilder WithNext<TCommand>(Func<ComplexCommandBase?, IServiceProvider, TCommand> complexCommandProvider)
        where TCommand : ComplexCommandBase
    {
        _finalProvider = (next, sp) => _finalProvider(complexCommandProvider(next, sp), sp);
        return this;
    }

    public IServiceCollection Build()
    {
        return serviceCollection.AddSingleton<ICommand>(sp => _finalProvider(null, sp));
    }
}