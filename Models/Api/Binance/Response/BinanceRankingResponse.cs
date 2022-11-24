namespace MyTrader.Models.API.Binance.Response;

public class BinanceRankingTradersResponse
{
    public object futureUid { get; set; }
    public string nickName { get; set; }
    public string userPhotoUrl { get; set; }
    public int rank { get; set; }
    public double value { get; set; }
    public bool positionShared { get; set; }
    public string twitterUrl { get; set; }
    public string encryptedUid { get; set; }
    public object updateTime { get; set; }
    public int followerCount { get; set; }
}

public class BinanceRankingResponse
{
    public string code { get; set; }
    public object message { get; set; }
    public object messageDetail { get; set; }
    public List<BinanceRankingTradersResponse> data { get; set; }
    public bool success { get; set; }
}