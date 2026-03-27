using Discord;
using Discord.WebSocket;
using Safira.Core;
using Safira.Services;

namespace Safira.Commands.Economy;

public class BalanceCommand(ExtendedClient client, EconomyService economy) : SlashCommandBase(client)
{
    public override string Name => "balance";
    public override string Description => "Check yours or another user current coin balance.";

    public override async Task Execute(SocketSlashCommand command)
    {
        var user = command.Data.Options.FirstOrDefault()?.Value as SocketGuildUser ?? (SocketGuildUser)command.User;
        var wallet = await economy.GetOrCreateWalletAsync(user.Id, command.GuildId!.Value);

        var message = $"""
        → {user.Mention}'s wallet
        ↳ **{wallet.Balance.ToCompact()}** coins.
        """;
        await command.RespondAsync(message);
    }

    public override Task<SlashCommandBuilder> BuildCommand()
    {

        var builder = new SlashCommandBuilder()
        .WithName(Name)
        .WithDescription(Description)
        .AddOption("user", ApplicationCommandOptionType.User, "The user who's wallet is going to be seen.", isRequired: false);

        return Task.FromResult(builder);
    }
}
