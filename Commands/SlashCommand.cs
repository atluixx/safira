using Discord;
using Discord.Net;
using Newtonsoft.Json;
using Safira.Core;

namespace Safira.Commands;

public class SlashCommand(ExtendedClient client, string name, string description)
{
    private readonly ExtendedClient _client = client;
    private readonly string _name = name;
    private readonly string _description = description;

    private SlashCommandProperties BuildCommand()
    {
        return new SlashCommandBuilder()
            .WithName(_name)
            .WithDescription(_description)
            .Build();
    }

    public async Task RegisterGlobal()
    {
        try
        {
            await _client.CreateGlobalApplicationCommandAsync(BuildCommand());
        }
        catch (HttpException ex)
        {
            Console.WriteLine(JsonConvert.SerializeObject(ex.Errors, Formatting.Indented));
        }
    }

    public async Task RegisterGuild(ulong guildId)
    {
        var guild = _client.GetGuild(guildId);

        if (guild is null)
        {
            Console.WriteLine($"Guild {guildId} not found.");
            return;
        }

        try
        {
            await guild.CreateApplicationCommandAsync(BuildCommand());
        }
        catch (HttpException ex)
        {
            Console.WriteLine(JsonConvert.SerializeObject(ex.Errors, Formatting.Indented));
        }
    }
}
