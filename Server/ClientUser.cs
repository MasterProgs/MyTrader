namespace MyTrader.Server;

using LiteNetwork.Server;
using MyTrader.Models.BOT;
using MyTrader.Models.Client;
using MyTrader.Models.Client.Command;

public class ClientUser : LiteServerUser
{
    private bool _isReady;
    public bool IsReady => _isReady;

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        try
        {
            var message = CommandManager.DecodeCommand<Commands>(packetBuffer);

            switch (message.Command)
            {
                case CommandEnum.FILTER:
                    var filterCommand = CommandManager.DecodeCommand<FilterCommand>(packetBuffer);
                    CommandManager.UpdateFilters(filterCommand, this);
                    break;
                case CommandEnum.TEST:
                    var testCommand = CommandManager.DecodeCommand<TestCommand>(packetBuffer);
                    List<Position> positions = new List<Position>();
                    if (testCommand.Type == "OPEN")
                    {
                        positions.Add(new Position()
                        {
                            Uid = 1,
                            PreciseLeverage = 7d,
                            Symbol = "BTCUSDT",
                            EntryPrice = 21000,
                            Side = PositionSide.LONG
                        });
                        positions.ForEach(x => this.Send(x.Encode(PositionType.OPEN)));
                    }
                    else
                    {
                        positions.Add(new Position()
                        {
                            Uid = 1,
                            PreciseLeverage = 7d,
                            Symbol = "BTCUSDT",
                            EntryPrice = 21000,
                            Side = PositionSide.LONG
                        });
                        positions.ForEach(x => this.Send(x.Encode(PositionType.CLOSE)));
                    }
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Task.CompletedTask;
    }

    protected override void OnConnected()
    {
        Console.WriteLine("New client");
    }

    protected override void OnDisconnected()
    {
        Console.WriteLine("Client left");
    }
}