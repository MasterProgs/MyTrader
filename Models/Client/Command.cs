using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace MyTrader.Models.Client;

public enum CommandEnum {
    FILTER,
    TEST
}

public class Commands {
    [JsonConverter(typeof(StringEnumConverter))]
    public CommandEnum Command { get; set; }
}