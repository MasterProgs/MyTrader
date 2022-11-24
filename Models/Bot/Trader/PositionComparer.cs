using System.Diagnostics.CodeAnalysis;

namespace MyTrader.Models.BOT.Trader;

public class PositionComparer : IEqualityComparer<Position> {

    public virtual bool Equals(Position? x, Position? y) {
        if(x == null || y == null)
            return false;
        return x.GetHashCode() == y.GetHashCode();
    }

    public int GetHashCode([DisallowNull] Position obj) {
        return obj.Uid;
    }
}