using System.Text;
using MyTrader.Models.Client;
using MyTrader.Models.Client.Command;
using Newtonsoft.Json;

namespace MyTrader.Server;

public static class CommandManager
{
    private const byte HEADER_SIZE = 4;

    private static Dictionary<string, Type> _commands = new Dictionary<string, Type>();
    private static Server _server;

    public static void Initialize(Server server)
    {
        _server = server;

        var type = typeof(Commands);
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => !p.IsAbstract && type.IsAssignableFrom(p));

        foreach (var item in types)
        {
            _commands.Add(item.Name.Replace("Argument", string.Empty).ToLower(), item);
        }
    }

    public static async void UpdateFilters(FilterCommand command, ClientUser user) {
        await _server.AddFilters(command.Filters);
    }

    public static T DecodeCommand<T>(byte[] buffer)
    {
        Console.WriteLine(Encoding.ASCII.GetString(buffer));
        return JsonConvert.DeserializeObject<T>(Encoding.ASCII.GetString(buffer));
    }
}