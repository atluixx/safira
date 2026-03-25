using System.Reflection;
using Safira.Commands;
using Safira.Core;

namespace Safira.Services;

public class CommandService
{
    private readonly ExtendedClient _client;
    public List<SlashCommandBase> Commands { get; } = [];

    public CommandService(ExtendedClient client)
    {
        _client = client;
        var commandTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(SlashCommandBase)) && !t.IsAbstract);

        foreach (var type in commandTypes)
        {
            var commandInstance = (SlashCommandBase)Activator.CreateInstance(type, _client)!;
            Commands.Add(commandInstance);
        }
    }

    public async Task RegisterAllCommands()
    {
        foreach (var command in Commands)
            await command.RegisterGlobal();
    }
}

