# MyTrader

Mytrader is a realtime scraper of CEX to detect trades from leaderboard.

## Compatible with :
- Binance

## How it works?

`dotnet run` ~ Server started at port <b>4444</b>

<i>You can now etablished a connection to listen trades</i>

## Packet composition
`<length(4bytes):payload(length)>` ~ payload type : JSON

## Example of packet :
```json
{
  "uid":29374923,
  "symbol":"ETHUSDT",
  "side":"SHORT",
  "type":"OPEN",
  "entryPrice":1600.02,
  "leverage":4
}
{
  "uid":29374923,
  "symbol":"ETHUSDT",
  "side":"CLOSE",
  "type":"OPEN",
  "entryPrice":1600.02,
  "leverage":4
}
{
  "uid":25834947,
  "symbol":"BTCUSDT",
  "side":"LONG",
  "type":"OPEN",
  "entryPrice":20000.59,
  "leverage":15
}
```

## Client Command
Once connected you can send commands to the server.

### Filter command
```json
{
  "command": "filter",
  "filters": [
    {
      "period": "WEEKLY",
      "sort": "PNL",
      "pnlGainType": "LEVEL2",
      "roiGainType": "LEVEL2",
    },
    {
      "period": "MONTHLY",
      "sort": "PNL",
      "pnlGainType": "LEVEL2",
      "roiGainType": "LEVEL2",
    }
  ]
}
```

## TODO
- Credentials access to the server
- Apply filter for each client, not a general filter
- Security in case the server crash
