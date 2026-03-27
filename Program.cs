using Discord;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Safira.Core;
using Safira.Data;
using Safira.Services;

namespace Safira;

public class Program
{
    public static async Task Main()
    {
        Env.Load();

        var token = Environment.GetEnvironmentVariable("SAFIRA_SECRET")
                    ?? throw new InvalidOperationException("SAFIRA_SECRET is not set.");
        var connStr = Environment.GetEnvironmentVariable("SAFIRA_DB")
                      ?? throw new InvalidOperationException("SAFIRA_DB is not set.");

        var services = new ServiceCollection();
        services.AddSingleton(ExtendedClient.Init());
        services.AddDbContext<SafiraDbContext>(opt => opt.UseNpgsql(connStr));
        services.AddScoped<EconomyService>();
        services.AddSingleton<CommandService>();
        services.AddSingleton<MessageService>();

        var provider = services.BuildServiceProvider();

        using (var scope = provider.CreateScope())
            await scope.ServiceProvider.GetRequiredService<SafiraDbContext>().Database.EnsureCreatedAsync();

        var client = provider.GetRequiredService<ExtendedClient>();
        var commandService = provider.GetRequiredService<CommandService>();
        var messageService = provider.GetRequiredService<MessageService>();

        client.Log += ConsoleLogger.Log;
        client.MessageReceived += messageService.ProcessMessage;
        client.Ready += async () =>
        {
            Console.WriteLine($"Connected as {client.CurrentUser}");
            await commandService.RegisterAllCommands();
        };

        client.SlashCommandExecuted += async command =>
        {
            await using var scope = provider.CreateAsyncScope();
            await commandService.Execute(scope.ServiceProvider, command);
        };

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        await Task.Delay(-1);
    }
}
