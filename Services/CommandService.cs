using Discord.WebSocket;

namespace Safira.Services;

public class CommandService
{
    public static Task ProcessMessage(SocketMessage message)
    {
        return Task.CompletedTask;
    }
}
