using Microsoft.Extensions.DependencyInjection;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;

namespace Svintus.MovieNightMakerBot.Core.DependencyInjection;

public class ComplexCommandBuilder(IServiceCollection serviceCollection, Func<ComplexCommandBase?, ComplexCommandBase> complexCommandProvider)
{
    private Func<ComplexCommandBase?, ComplexCommandBase> _finalProvider = complexCommandProvider;
    
    public ComplexCommandBuilder WithNext<TCommand>(Func<ComplexCommandBase?, TCommand> complexCommandProvider)
        where TCommand : ComplexCommandBase
    {
        _finalProvider = next => _finalProvider(complexCommandProvider(next));
        return this;
    }

    public IServiceCollection Build()
    {
        return serviceCollection.AddSingleton<ICommand>(_ => _finalProvider(null));
    }
}