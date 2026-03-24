namespace Safira;

using Discord;
using DotNetEnv;
using Safira.Interfaces;
using Safira.Services;

public class Program
{
    private static readonly IExtendedClient Client = IExtendedClient.Init();

    public static async Task Main()
    {
        Env.Load();
        var secret = Environment.GetEnvironmentVariable("SAFIRA_SECRET") ?? throw new InvalidOperationException("SAFIRA_SECRET is not set.");

        Client.Log += ILogger.Log;
        Client.MessageReceived += CommandService.ProcessMessage;

        await Client.LoginAsync(TokenType.Bot, secret);
        await Client.StartAsync();

        await Task.Delay(-1);
    }
}
