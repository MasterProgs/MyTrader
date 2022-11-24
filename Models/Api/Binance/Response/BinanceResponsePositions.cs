namespace MyTrader.Models.API.Binance.Response;

public class Data
{
    public List<OtherPositionRetList> otherPositionRetList { get; set; }
    public List<int> updateTime { get; set; }
    public object updateTimeStamp { get; set; }
}

public class OtherPositionRetList
{
    public string symbol { get; set; }
    public double entryPrice { get; set; }
    public double markPrice { get; set; }
    public double pnl { get; set; }
    public double roe { get; set; }
    public List<int> updateTime { get; set; }
    public double amount { get; set; }
    public long updateTimeStamp { get; set; }
    public bool yellow { get; set; }
    public bool tradeBefore { get; set; }
}

public class BinanceResponsePositions
{
    public string code { get; set; }
    public object message { get; set; }
    public object messageDetail { get; set; }
    public Data data { get; set; }
    public bool success { get; set; }
}