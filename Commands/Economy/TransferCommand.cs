using Discord;
using Discord.WebSocket;
using Safira.Core;
using Safira.Services;

namespace Safira.Commands.Economy;

public class TransferCommand(ExtendedClient client, EconomyService economy) : SlashCommandBase(client)
{
    public override string Name => "transfer";
    public override string Description => "Send coins to another user.";

    public override Task<SlashCommandBuilder> BuildCommand()
    {
        var builder = new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription(Description)
            .AddOption("user", ApplicationCommandOptionType.User, "The user to send coins to.", isRequired: true)
            .AddOption("amount", ApplicationCommandOptionType.String, "Amount of coins to send.", isRequired: true);

        return Task.FromResult(builder);
    }

    public override async Task Execute(SocketSlashCommand command)
    {
        var target = (IUser)command.Data.Options.First(o => o.Name == "user").Value;
        var amountRaw = (string)command.Data.Options.First(o => o.Name == "amount").Value;
        var amount = (long)amountRaw.FromCompact();

        var (success, error) = await economy.TryTransferAsync(
            command.User.Id, target.Id, command.GuildId!.Value, amount);

        if (!success)
        {
            await command.RespondAsync($"❌ {error}", flags: MessageFlags.Ephemeral);
            return;
        }

        var embed = new EmbedBuilder()
            .WithTitle("💸 Transfer Complete")
            .WithDescription($"{command.User.Mention} sent **{amount.ToCompact()} coins** to {target.Mention}.")
            .WithColor(Color.Blue)
            .Build();

        await command.RespondAsync(embed: embed);
    }
}
