using Discord;
using Discord.WebSocket;

namespace Safira.Interfaces;

public class IExtendedClient : DiscordSocketClient
{
    public IExtendedClient()
    {
    }

    public IExtendedClient(DiscordSocketConfig config) : base(config)
    {
    }

    public static IExtendedClient Init()
    {
        return new(new()
        {
            GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.GuildMembers | GatewayIntents.MessageContent
        });
    }


}
