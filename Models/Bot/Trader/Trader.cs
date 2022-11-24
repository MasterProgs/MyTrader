namespace MyTrader.Models.BOT.Trader;

public abstract class Trader {
    public string NickName { get; set; }
    public Dictionary<int, Position> Positions { get; set; }
    public double TotalPNL { get; set; }
    public double TotalROI { get; set; }

    public Trader() {
        this.Positions = new Dictionary<int, Position>();
    }

    protected List<Position> GetNewPositions(Dictionary<int, Position> newPositions) {
        return newPositions.Values.Where(kvp => !this.Positions.ContainsKey(kvp.Uid)).ToList();
    }

    protected List<Position> GetClosedPositions(Dictionary<int, Position> newPositions) {
        return this.Positions.Values.Where(kvp => !newPositions.ContainsKey(kvp.Uid)).ToList();
    }
}