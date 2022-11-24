using System.Text;
using LiteNetwork.Server;
using MyTrader.Server;

class Program
{
    static async Task Main(string[] args)
    {
        //Create the server configuration, to listen on "127.0.0.1" and port "4444"
        var configuration = new LiteServerOptions()
        {
            Host = "127.0.0.1",
            Port = 4444
        };

        //Create the server instance by givin the server options and start it.
        // args[0] = core
        var server = new Server(configuration, int.Parse(args[0]));
        await server.Start();
    }
}