using Discord;
using Discord.WebSocket;
using Safira.Core;

namespace Safira.Commands;

public abstract class SlashCommandBase(ExtendedClient client)
{
    protected readonly ExtendedClient Client = client;

    public abstract string Name { get; }
    public abstract string Description { get; }

    public virtual Task<SlashCommandBuilder> BuildCommand()
    {
        var builder = new SlashCommandBuilder()
            .WithName(Name.ToLower())
            .WithDescription(Description);
        return Task.FromResult(builder);
    }

    public async Task RegisterGlobal(string? nameOverride = null)
    {
        var builder = await BuildCommand();
        if (nameOverride != null)
            builder.WithName(nameOverride.ToLower());
        await Client.CreateGlobalApplicationCommandAsync(builder.Build());
    }

    public async Task RegisterGuild(ulong guildId, string? nameOverride = null)
    {
        var guild = Client.GetGuild(guildId);
        if (guild == null) return;

        var builder = await BuildCommand();
        if (nameOverride != null)
            builder.WithName(nameOverride.ToLower());
        await guild.CreateApplicationCommandAsync(builder.Build());
    }

    public abstract Task Execute(SocketSlashCommand command);
}
