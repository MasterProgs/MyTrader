using MyTrader.Models.API.Binance.Payload;

namespace MyTrader.Models.BOT.Trader.Binance;

public class BinancePerformance {
    public BinanceRankingPeriodTypeEnum PeriodType { get; set; }
    public BinanceRankingStatsTypeEnum StatisticsType { get; set; }
    public double Value { get; set; }
    public int Rank { get; set; }
}