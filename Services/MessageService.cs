using Discord.WebSocket;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Safira.Core;

namespace Safira.Services;

public class MessageService(ExtendedClient client)
{
    private readonly ExtendedClient _client = client;

    public async Task ProcessMessage(SocketMessage message)
    {
        if (string.IsNullOrEmpty(message.Content) ||
            message.Author.Id == _client.CurrentUser.Id)
            return;

        Console.WriteLine(
            $"[{message.Author.Username} | {message.Channel.Name}] : {message.Content}"
        );

        return;
    }
}
