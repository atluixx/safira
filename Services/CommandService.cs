using Safira.Commands;
using Safira.Core;

namespace Safira.Services;

public class CommandService(ExtendedClient client)
{
    private readonly ExtendedClient _client = client;

    public async Task RegisterCommands()
    {
        var ping = new SlashCommand(_client, "ping", "Replies with pong");

        await ping.RegisterGlobal();
    }
}
