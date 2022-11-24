using LiteNetwork.Client;
using MyTrader.Models.BOT;

namespace MyTrader.Server;

public class ClientTest : LiteClient
{
    public ClientTest(LiteClientOptions options, IServiceProvider? serviceProvider = null) : base(options, serviceProvider)
    {

    }

    protected override void OnConnected()
    {
        Console.WriteLine("Conncted");
    }

    protected override void OnDisconnected()
    {
        Console.WriteLine("Disconnected");
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        Console.WriteLine(packetBuffer.Length);
        var position = Position.Decode(packetBuffer);
        Console.WriteLine(position);
        return Task.CompletedTask;
    }
}