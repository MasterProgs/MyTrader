using MyTrader.Models.API.Binance.Payload;
using MyTrader.Models.API.Binance.Response;
using MyTrader.Models.BOT;
using MyTrader.Models.BOT.Trader.Binance;

namespace MyTrader.Scraper;

public class BinanceScraper : Scraper
{
    public BinanceScraper() { }

    public async Task<List<BinanceTrader>> SearchTraders(BinanceRankingStatsTypeEnum sort, BinanceRankingPeriodTypeEnum period, BinanceTradeTypeEnum type, GainTypeEnum pnlGainType, GainTypeEnum roiGainType, string symbol = "")
    {
        var payload = new BinanceRankingSearchPayload() 
        {
            isShared = true,
            sortType = sort,
            periodType = period,
            tradeType = type,
            pnlGainType = pnlGainType,
            roiGainType = roiGainType,
            symbol = symbol
        };

        var result = await _httpClient.PostAsync(BinanceRoutes.SearchLeaderboardRoute, GeneratePayload(payload));
        var json = await result.Content.ReadAsStringAsync();
        var response = GenerateResponse<BinanceRankingSearchResponse>(json);

        var traders = new List<BinanceTrader>();

        response.data.ForEach(x => traders.Add(new BinanceTrader()
        {
            NickName = x.nickName,
            EncryptedUid = x.encryptedUid,
            Rank = x.rank
        }));

        return traders;
    }

    public async Task<List<BinanceTrader>> GetTraders(BinanceRankingStatsTypeEnum sort, BinanceRankingPeriodTypeEnum period, BinanceTradeTypeEnum type)
    {
        var payload = new BinanceRankingPayload()
        {
            isShared = true,
            statisticsType = sort,
            periodType = period,
            tradeType = type
        };

        var result = await _httpClient.PostAsync(BinanceRoutes.GetLeaderboardRoute, GeneratePayload(payload));
        var json = await result.Content.ReadAsStringAsync();
        var response = GenerateResponse<BinanceRankingResponse>(json);

        var traders = new List<BinanceTrader>();

        response.data.ForEach(x => traders.Add(new BinanceTrader()
        {
            NickName = x.nickName,
            EncryptedUid = x.encryptedUid,
            Rank = x.rank
        }));

        return traders;
    }

    public async Task<List<BinancePerformance>> GetPerformances(BinanceTrader trader, BinanceTradeTypeEnum type)
    {
        var payload = new BinanceTraderPayload()
        {
            encryptedUid = trader.EncryptedUid,
            tradeType = type
        };

        try
        {
            var result = await _httpClient.PostAsync(BinanceRoutes.PerformancesRoute, GeneratePayload(payload));
            var json = await result.Content.ReadAsStringAsync();
            var response = GenerateResponse<BinancePerformanceResponse>(json);

            var performances = new List<BinancePerformance>();

            response.data.ForEach(x =>
            {
                performances.Add(
                    new BinancePerformance
                    {
                        PeriodType = Enum.Parse<BinanceRankingPeriodTypeEnum>(x.periodType),
                        StatisticsType = Enum.Parse<BinanceRankingStatsTypeEnum>(x.statisticsType),
                        Value = x.value,
                        Rank = x.rank
                    }
                );
            }
            );

            return performances;
        }
        catch
        {  // => Error?
            Console.WriteLine("Rate limited");
            return trader.Performances;
        }
    }

    public async Task<Dictionary<int, Position>> GetPositions(BinanceTrader trader, BinanceTradeTypeEnum type)
    {
        var payload = new BinanceTraderPayload()
        {
            encryptedUid = trader.EncryptedUid,
            tradeType = type
        };

        try
        {
            var result = await _httpClient.PostAsync(BinanceRoutes.PositionsRoute, GeneratePayload(payload));

            if(result.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new Exception("Rate Limited");

            var json = await result.Content.ReadAsStringAsync();
            var response = GenerateResponse<BinanceResponsePositions>(json);

            if(response.data.otherPositionRetList == null) {
                throw new ArgumentException("User settings changed");
            }

            var positions = new Dictionary<int, Position>();
            response.data.otherPositionRetList.ToList().ForEach(x =>
            {
                var side = x.amount > 0 ? PositionSide.LONG : PositionSide.SHORT;
                var uid = (trader.EncryptedUid + x.symbol + side).GetHashCode();
                var updateDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(x.updateTimeStamp);
                positions[uid] = new Position()
                {
                    Uid = uid,
                    Symbol = x.symbol,
                    Side = side,
                    UpdateDate = updateDate,
                    ROI = x.roe,
                    PNL = x.pnl,
                    EntryPrice = x.entryPrice,
                    MarketPrice = x.markPrice,
                    PreciseLeverage = x.roe != 0d ? Math.Abs(x.roe / ((x.markPrice - x.entryPrice) / x.markPrice)) : -1,
                    SizeTokenA = Math.Abs(x.amount),
                    SizeTokenB = Math.Abs(x.amount / x.entryPrice)
                };
            });

            return positions;
        }
        catch
        { 
            throw new TimeoutException("Network or API slow");
        }
    }
}