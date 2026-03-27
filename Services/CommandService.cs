using System.Reflection;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Safira.Commands;

namespace Safira.Services;

public class CommandService
{
    private readonly IServiceProvider _services;
    private readonly Dictionary<string, Type> _commandMap = new(StringComparer.OrdinalIgnoreCase);

    public CommandService(IServiceProvider services)
    {
        _services = services;

        var commandTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(SlashCommandBase)) && !t.IsAbstract);

        foreach (var type in commandTypes)
        {
            using var scope = services.CreateScope();
            var instance = (SlashCommandBase)ActivatorUtilities.CreateInstance(scope.ServiceProvider, type);
            _commandMap[instance.Name] = type;
        }
    }

    public async Task RegisterAllCommands()
    {
        var registered = new HashSet<Type>();
        foreach (var (_, type) in _commandMap)
        {
            if (!registered.Add(type)) continue;

            using var scope = _services.CreateScope();
            var instance = (SlashCommandBase)ActivatorUtilities.CreateInstance(scope.ServiceProvider, type);

            await instance.RegisterGlobal();
        }
    }

    public async Task Execute(IServiceProvider scopedServices, SocketSlashCommand command)
    {
        if (!_commandMap.TryGetValue(command.CommandName, out var type)) return;
        var instance = (SlashCommandBase)ActivatorUtilities.CreateInstance(scopedServices, type);
        await instance.Execute(command);
    }
}
