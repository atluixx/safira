using Discord;
using Discord.WebSocket;
using Safira.Core;

namespace Safira.Commands
{
    public abstract class SlashCommandBase(ExtendedClient client)
    {
        protected readonly ExtendedClient Client = client;

        public abstract string Name { get; }
        public abstract string Description { get; }

        public async Task RegisterGlobal()
        {
            var command = new SlashCommandBuilder()
                .WithName(Name.ToLower())
                .WithDescription(Description)
                .Build();

            await Client.CreateGlobalApplicationCommandAsync(command);
        }

        public async Task RegisterGuild(ulong guildId)
        {
            var guild = Client.GetGuild(guildId);
            if (guild == null) return;

            var command = new SlashCommandBuilder()
                .WithName(Name.ToLower())
                .WithDescription(Description)
                .Build();

            await guild.CreateApplicationCommandAsync(command);
        }

        public abstract Task Execute(SocketSlashCommand command);
    }
}
