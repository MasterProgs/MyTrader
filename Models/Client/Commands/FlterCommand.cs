using MyTrader.Models.API.Binance.Payload;

namespace MyTrader.Models.Client.Command;

public class FilterCommand : Commands {
    public List<FilterArgument> Filters { get; set; }
}

public class FilterArgument {
    public BinanceRankingPeriodTypeEnum Period { get; set; }
    public BinanceRankingStatsTypeEnum Sort { get; set; }
    public BinanceTradeTypeEnum TradeType { get; set; }
    public GainTypeEnum PnlGainType { get; set; }
    public GainTypeEnum RoiGainType { get; set; }
}