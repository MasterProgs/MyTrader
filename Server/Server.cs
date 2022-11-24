using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using LiteNetwork.Server;
using MyTrader.Models.API.Binance.Payload;
using MyTrader.Models.BOT;
using MyTrader.Models.BOT.Trader.Binance;
using MyTrader.Models.Client.Command;
using MyTrader.Scraper;

namespace MyTrader.Server;

public class Server : LiteServer<ClientUser>
{
    static ConcurrentDictionary<string, BinanceTrader> binanceTraders = new ConcurrentDictionary<string, BinanceTrader>();
    private BinanceScraper _binanceScraper;
    private int _iCoreCount;
    private ParallelOptions _parrallelOptions;
    private bool _active;

    public Server(LiteServerOptions options, int iCoreCount, IServiceProvider serviceProvider = null)
        : base(options, serviceProvider)
    {
        this._binanceScraper = new BinanceScraper();
        this._iCoreCount = iCoreCount;
        // => 1req = 150ms. Limit rate API. 1200req/min, 20req/sec, 1req/50ms.
        this._parrallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = _iCoreCount
        };
        this._active = true;
    }

    public async Task AddFilters(List<FilterArgument> filters)
    {
        Pause();

        binanceTraders = new ConcurrentDictionary<string, BinanceTrader>();

        // => Get Best Traders
        await Parallel.ForEachAsync(filters, _parrallelOptions, async (filter, ct) =>
        {
            var traders = await _binanceScraper.SearchTraders(filter.Sort, filter.Period, BinanceTradeTypeEnum.PERPETUAL, filter.PnlGainType, filter.RoiGainType);
            var newTraders = traders.Where(x => !binanceTraders.ContainsKey(x.EncryptedUid)).ToList();
            newTraders.ForEach(x => binanceTraders[x.EncryptedUid] = x);
        });

        await PrepareServer();
        
        Resume();
    }

    private async Task PrepareServer()
    {
        // // => Get Current Performances
        // await Parallel.ForEachAsync(binanceTraders.Values, optionsDefault, async (trader, ct) =>
        // {
        //     trader.Performances = await _binanceScraper.GetPerformances(trader, tradeType);
        // });

        // // => Remove bad traders
        // binanceTraders = new ConcurrentDictionary<string, BinanceTrader>(
        //     binanceTraders.Where(x => x.Value.Performances.Any(x => x.PeriodType == BinanceRankingPeriodTypeEnum.DAILY && x.StatisticsType == BinanceRankingStatsTypeEnum.ROI && x.Value > .05d)).ToDictionary(x => x.Key, x => x.Value)
        // );

        // => Get Current Positions
        await Parallel.ForEachAsync(binanceTraders.Values, _parrallelOptions, async (trader, ct) =>
        {
            try
            {
                trader.Positions = await _binanceScraper.GetPositions(trader, BinanceTradeTypeEnum.PERPETUAL);
            }
            catch (Exception e)
            {
                if (e is ArgumentException)
                {
                    if (binanceTraders.TryRemove(trader.EncryptedUid, out trader))
                        Console.WriteLine($"({trader.NickName}) - {e.Message} - Removed");
                }
            }
        });

        Console.WriteLine($"Detection of {binanceTraders.Count} Traders");
        Console.WriteLine($"Initiale Positions [OK]");
    }

    public void PrepareManager()
    {
        CommandManager.Initialize(this);
    }

    public void SendToAll() {
        
    }

    public async Task Start()
    {
        // => Managers
        PrepareManager();
        await StartAsync();

        var tradeType = BinanceTradeTypeEnum.PERPETUAL;

        Console.WriteLine($"> Start");
        var errors = 0;

        while (errors < 10)
        {
            if(!_active || Users.Count() == 0 || binanceTraders.Count == 0) {
                //Console.WriteLine($"No filters or users connected or pause ... Waiting 5sec");
                Thread.Sleep(5000);
                continue;
            }

            //Console.WriteLine($"Analyze of {binanceTraders.Count} Traders ...");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await Parallel.ForEachAsync(binanceTraders.Values, _parrallelOptions, async (trader, ct) =>
            {
                try
                {
                    var currentPositions = await _binanceScraper.GetPositions(trader, tradeType);

                    var newPositions = trader.DetectNewPositions(currentPositions);
                    if (newPositions.Count > 0)
                    {
                        trader.Positions.Values.Where(x => newPositions.Any(y => y.Symbol == x.Symbol)).ToList().ForEach(x => Console.WriteLine($"[OLD] | {x}"));
                        newPositions.ForEach(x =>
                        {
                            SendToAll(x.Encode(PositionType.OPEN));
                            Console.WriteLine($"<{x.UpdateDate.ToLongTimeString()}> [OPEN - {x.Symbol} - {x.Type}] Trade of `{trader.NickName}` {x}");
                        });
                    }

                    trader.AddDetectPositions(newPositions);
                    var closedPositions = trader.DetectClosedPositions(currentPositions);

                    if (closedPositions.Count > 0)
                    {
                        closedPositions.ForEach(x =>
                        {
                            SendToAll(x.Encode(PositionType.CLOSE));
                            Console.WriteLine($"<{x.UpdateDate.ToLongTimeString()}> [CLOSE - {x.Symbol} - {x.Type}] Trade of `{trader.NickName}` | [{x.Symbol}] | ROI: {x.ROI.ToString("P", CultureInfo.InvariantCulture)} | Profits: ${x.PNL} | Price {x.EntryPrice.ToString("0.000000")}/{x.MarketPrice.ToString("0.000000")}");
                        });
                    }

                    trader.RemoveClosedPositions(closedPositions);
                }
                catch (Exception e)
                {
                    if (e.InnerException is not TimeoutException)
                        errors++;
                }
                //Console.WriteLine($"`{trader.NickName}` | TotalPNL = ${trader.TotalPNL.ToString("0.00")} | TotalRoi = {trader.TotalROI.ToString("P", CultureInfo.InvariantCulture)}");
            });

            stopWatch.Stop();
            //Console.WriteLine($"Finished in {stopWatch.Elapsed.TotalMilliseconds.ToString("0.00")} ms");
            var tempAvgReqMs = stopWatch.Elapsed.TotalMilliseconds / binanceTraders.Count;
            //Console.WriteLine($"Avg req {(tempAvgReqMs).ToString("0.00")} ms");
            if (tempAvgReqMs < 300)
            {
                var waitDurationMs = (300 - (int)tempAvgReqMs) * binanceTraders.Count;
                //Console.WriteLine($"Security wait {waitDurationMs.ToString("0.000")} ms");
                Thread.Sleep(waitDurationMs);
            }

            errors = 0;
        }

        Console.WriteLine($"> End {errors} errors");
    }

    private void Pause() {
        this._active = false;
    }

    private void Resume() {
        this._active = true;
    }
}