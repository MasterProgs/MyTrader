namespace MyTrader.Models.API.Binance.Payload;

public enum BinanceTradeTypeEnum {
    PERPETUAL, //USD-M
    DELIVERY  // COIN-M
}

public class BinanceTraderPayload : IPayload
{
    public string encryptedUid { get; set; }
    public BinanceTradeTypeEnum tradeType { get; set; }
}