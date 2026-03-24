using Discord;

namespace Safira.Interfaces;

public class ILogger
{
    public static Task Log(LogMessage logMessage)
    {
        Console.WriteLine(logMessage.ToString());
        return Task.CompletedTask;
    }
}
