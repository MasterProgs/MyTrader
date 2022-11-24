namespace MyTrader.Scraper;

public class BinanceRoutes {
    public static string PositionsRoute => "https://www.binance.com/bapi/futures/v1/public/future/leaderboard/getOtherPosition";
    public static string PerformancesRoute => "https://www.binance.com/bapi/futures/v1/public/future/leaderboard/getOtherPerformance";
    public static string GetLeaderboardRoute => "https://www.binance.com/bapi/futures/v2/public/future/leaderboard/getLeaderboardRank";
    public static string SearchLeaderboardRoute => "https://www.binance.com/bapi/futures/v1/public/future/leaderboard/searchLeaderboard";
}