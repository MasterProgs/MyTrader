using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyTrader.Models.BOT;

public enum PositionType {
    OPEN,
    CLOSE
}

public enum PositionSide {
    SHORT,
    LONG
}

public class Position {

    private const byte HEADER_SIZE = 4;
    public int Uid {get; set; }
    public string Symbol { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public PositionSide Side { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public PositionType Type { get; set; }
    [JsonIgnore]
    public DateTime UpdateDate { get; set; }
    public double EntryPrice { get; set; }
    [JsonIgnore]
    public double MarketPrice { get; set; }
    [JsonIgnore]
    public double PreciseLeverage { get; set; }
    public int Leverage => (int)Math.Round(PreciseLeverage);
    [JsonIgnore]
    public double ROI { get; set; }
    [JsonIgnore]
    public double PNL { get; set; }
    [JsonIgnore]
    public double SizeTokenA { get; set; }
    [JsonIgnore]
    public double SizeTokenB { get; set; }
    [JsonIgnore]
    public double Profits => PNL/PreciseLeverage;

    public override string ToString()
    {
        return $"{this.Symbol} | Entry:{this.EntryPrice} | SizeA:{this.SizeTokenA} | SizeB:{this.SizeTokenB} | Leverage:{this.Leverage} | Roi:{this.ROI} | Update:{this.UpdateDate.Millisecond}";
    }

    public byte[] Encode(PositionType type)
    {
        this.Type = type;
        var formatter = new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() };
        Console.WriteLine(JsonConvert.SerializeObject(this, formatter));
        return Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(this, formatter));
    }

    public static Position Decode(byte[] buffer) {
        return JsonConvert.DeserializeObject<Position>(Encoding.ASCII.GetString(buffer));
    }
}