using Discord;
using Discord.WebSocket;
using Safira.Core;
using Safira.Services;

namespace Safira.Commands.Economy;

public class DailyCommand(ExtendedClient client, EconomyService economy) : SlashCommandBase(client)
{
    public override string Name => "daily";
    public override string Description => "Claim your daily 100 coins.";

    public override async Task Execute(SocketSlashCommand command)
    {
        var (success, timeRemaining) = await economy.TryClaimDailyAsync(
            command.User.Id, command.GuildId!.Value);

        if (!success)
        {
            var hours = (int)timeRemaining!.Value.TotalHours;
            var minutes = timeRemaining.Value.Minutes;

            await command.RespondAsync(
                $"You already claimed your daily coins. Come back in **{hours}h {minutes}m**.",
                flags: MessageFlags.Ephemeral);
            return;
        }

        var embed = new EmbedBuilder()
            .WithTitle("✅ Daily Claimed!")
            .WithDescription($"{command.User.Mention} received **100 coins**.")
            .WithColor(Color.Green)
            .Build();

        await command.RespondAsync(embed: embed);
    }
}
