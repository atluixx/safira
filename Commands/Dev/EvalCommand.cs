using Discord;
using Discord.WebSocket;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Safira.Core;
using Safira.Data;

namespace Safira.Commands.Dev;

public class EvalGlobals
{
    public required SocketSlashCommand Command { get; init; }
    public required SafiraDbContext Db { get; init; }
    public required ExtendedClient Client { get; init; }
}

public class EvalCommand(ExtendedClient client, SafiraDbContext db) : SlashCommandBase(client)
{
    private const ulong OwnerId = 1298772912848769075;

    private static readonly ScriptOptions ScriptOptions = ScriptOptions.Default
        .AddImports(
            "System",
            "System.Linq",
            "System.Collections.Generic",
            "System.Threading.Tasks",
            "Discord",
            "Discord.WebSocket",
            "Safira.Data",
            "Safira.Data.Models"
        )
        .AddReferences(
            typeof(object).Assembly,
            typeof(Enumerable).Assembly,
            typeof(ExtendedClient).Assembly,
            typeof(SafiraDbContext).Assembly
        );

    public override string Name => "eval";
    public override string Description => "Evaluate a C# script. (Owner only)";

    public override Task<SlashCommandBuilder> BuildCommand()
    {
        var builder = new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription(Description)
            .WithDefaultMemberPermissions(GuildPermission.Administrator)
            .AddOption("code", ApplicationCommandOptionType.String, "C# code to evaluate.", isRequired: true);

        return Task.FromResult(builder);
    }

    public override async Task Execute(SocketSlashCommand command)
    {
        if (command.User.Id != OwnerId)
        {
            await command.RespondAsync("You are not allowed to use this command.", ephemeral: true);
            return;
        }

        var code = (string)command.Data.Options.First().Value;
        await command.DeferAsync(ephemeral: true);

        try
        {
            var globals = new EvalGlobals { Command = command, Db = db, Client = Client };
            var result = await CSharpScript.EvaluateAsync<object?>(code, ScriptOptions, globals, typeof(EvalGlobals));

            var output = result?.ToString() ?? "null";
            await command.FollowupAsync($"```\n{output}\n```", ephemeral: true);
        }
        catch (Exception ex)
        {
            await command.FollowupAsync($"```\n{ex.Message}\n```", ephemeral: true);
        }
    }
}
