using System.Net.Http.Headers;
using System.Text;
using MyTrader.Models.API.Binance.Payload;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyTrader.Scraper;

public abstract class Scraper {

    protected HttpClient _httpClient;

    public Scraper() {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
    }

    protected StringContent GeneratePayload(IPayload payload) {
        var jsonFormatter = new JsonSerializerSettings();
        jsonFormatter.Converters.Add(new StringEnumConverter());
        return new StringContent(JsonConvert.SerializeObject(payload, jsonFormatter), Encoding.UTF8, "application/json");
    }

    protected T GenerateResponse<T>(string content) {
        return JsonConvert.DeserializeObject<T>(content);
    }
}