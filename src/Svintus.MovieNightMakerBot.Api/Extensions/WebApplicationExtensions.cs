using Svintus.MovieNightMakerBot.Core.BotSetup.Abstractions;

namespace Svintus.MovieNightMakerBot.Api.Extensions;

internal static class WebApplicationExtensions
{
    public static IApplicationBuilder UseBotSetup(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var botSetupService = scope.ServiceProvider.GetRequiredService<IBotSetupService>();
        botSetupService.SetupAsync().Wait();
        
        return app;
    }
}