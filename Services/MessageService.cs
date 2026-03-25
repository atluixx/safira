using Discord.WebSocket;
using Safira.Core;

namespace Safira.Services;

public class MessageService(ExtendedClient client)
{
    private readonly ExtendedClient _client = client;

    public Task ProcessMessage(SocketMessage message)
    {
        if (string.IsNullOrEmpty(message.Content) ||
            message.Author.Id == _client.CurrentUser.Id)
            return Task.CompletedTask;

        Console.WriteLine(
            $"[{message.Author.Username} | {message.Channel.Name}] : {message.Content}"
        );

        return Task.CompletedTask;
    }
}
