namespace MyTrader.Models.BOT.Trader.Binance;

public class BinanceTrader : Trader {
    public List<BinancePerformance> Performances { get; set; }
    public string EncryptedUid { get; set; }
    public int Rank { get; set; }

    public BinanceTrader() {
        this.Performances = new List<BinancePerformance>();
    }

    public List<Position> DetectNewPositions(Dictionary<int, Position> newPositionsDetected) {
        return this.GetNewPositions(newPositionsDetected);
    }

    public void AddDetectPositions(List<Position> newPostions) {
        newPostions.ForEach(x => this.Positions[x.Uid] = x);
    }

    public void RemoveClosedPositions(List<Position> closedPostions) {
        closedPostions.ForEach(x => this.Positions.Remove(x.Uid));
    }

    public List<Position> DetectClosedPositions(Dictionary<int, Position> newPositionsDetected) {
        return GetClosedPositions(newPositionsDetected);
    }
}