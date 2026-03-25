using Discord.WebSocket;
using Safira.Core;

namespace Safira.Commands.Utils;

public class PingCommand(ExtendedClient client) : SlashCommandBase(client)
{
    public override string Name => "ping";
    public override string Description => "Replies with pong!";

    public override async Task Execute(SocketSlashCommand command)
        => await command.RespondAsync("Pong!");
}
