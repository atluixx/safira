using Discord;

namespace Safira.Core;

public static class ConsoleLogger
{
    public static Task Log(LogMessage logMessage)
    {
        Console.WriteLine(logMessage.ToString());
        return Task.CompletedTask;
    }
}
