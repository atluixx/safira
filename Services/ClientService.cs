using Safira.Core;

namespace Safira.Services;

public class ClientService(ExtendedClient client)
{
    private readonly ExtendedClient _client = client;

    public Task Ready()
    {
        Console.WriteLine($"Connected as {_client.CurrentUser}");
        return Task.CompletedTask;
    }
}
