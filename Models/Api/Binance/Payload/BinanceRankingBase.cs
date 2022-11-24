namespace MyTrader.Models.API.Binance.Payload;

public enum BinanceRankingPeriodTypeEnum
{
    DAILY,
    WEEKLY,
    MONTHLY,
    YEARLY,
    ALL,
    EXACT_WEEKLY,
    EXACT_MONTHLY,
    EXACT_YEARLY
}

public enum BinanceRankingStatsTypeEnum
{
    PNL,
    ROI
}

public class BinanceRankingBase : IPayload
{
    public bool isShared { get; set; }
    public BinanceRankingPeriodTypeEnum periodType { get; set; }
    public BinanceTradeTypeEnum tradeType { get; set; }
}