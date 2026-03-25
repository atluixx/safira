using Discord;
using Discord.WebSocket;
using DotNetEnv;
using Safira.Core;
using Safira.Services;

namespace Safira;

public class Program
{
    private static readonly ExtendedClient Client = ExtendedClient.Init();
    private static CommandService CommandService { get; set; } = new(Client);
    private static MessageService MessageService { get; set; } = new(Client);

    public static async Task Main()
    {
        Env.Load();
        var token = Environment.GetEnvironmentVariable("SAFIRA_SECRET")
                    ?? throw new InvalidOperationException("SAFIRA_SECRET is not set.");

        Client.Log += ConsoleLogger.Log;
        Client.MessageReceived += MessageService.ProcessMessage;
        Client.Ready += async () =>
        {
            Console.WriteLine($"Connected as {Client.CurrentUser}");
            await CommandService.RegisterAllCommands();
        };

        Client.SlashCommandExecuted += HandleInteractions;

        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();

        await Task.Delay(-1);
    }

    private static async Task HandleInteractions(SocketSlashCommand command)
    {
        var slashCommand = CommandService.Commands.FirstOrDefault(c => c.Name == command.CommandName);
        if (slashCommand == null) return;

        await slashCommand.Execute(command);
    }
}
