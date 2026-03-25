using Discord;
using Discord.WebSocket;

namespace Safira.Core;

public class ExtendedClient : DiscordSocketClient
{
    public ExtendedClient()
    {
    }

    public ExtendedClient(DiscordSocketConfig config) : base(config)
    {
    }

    public static ExtendedClient Init()
    {
        return new ExtendedClient(new DiscordSocketConfig
        {
            GatewayIntents =
                GatewayIntents.GuildMessages |
                GatewayIntents.GuildMembers |
                GatewayIntents.Guilds |
                GatewayIntents.MessageContent
        });
    }
}
