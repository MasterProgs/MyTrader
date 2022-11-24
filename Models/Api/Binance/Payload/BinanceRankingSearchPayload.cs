namespace MyTrader.Models.API.Binance.Payload;

public enum GainTypeEnum { // PNL | ROI
    LEVEL1, // 0-1k$ | 0-50%
    LEVEL2, // 1k-10k$ | 50-100%
    LEVEL3, // 10-100k$ | 100-500%
    LEVEL4, // 100-1M$ | 500-1k%
    LEVEL5 // 1M$+ | 1k%+
}

public class BinanceRankingSearchPayload : BinanceRankingBase
{
    public BinanceRankingSearchPayload()
    {
        limit = 200;
    }
        
    public BinanceRankingStatsTypeEnum sortType { get; set; }
    public int limit { get; set; }
    public GainTypeEnum pnlGainType { get; set; }
    public GainTypeEnum roiGainType { get; set; }
    public string symbol { get; set; }
}