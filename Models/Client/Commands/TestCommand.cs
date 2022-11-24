using MyTrader.Models.API.Binance.Payload;

namespace MyTrader.Models.Client.Command;

public class TestCommand : Commands {
    public string Type { get; set; }
}