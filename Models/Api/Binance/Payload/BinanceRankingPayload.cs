namespace MyTrader.Models.API.Binance.Payload;

public class BinanceRankingPayload : BinanceRankingBase
{
    public BinanceRankingStatsTypeEnum statisticsType { get; set; }
}