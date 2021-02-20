using System.Text.Json.Serialization;

namespace Kubera.Business.Models.AlphaVantage
{
    public sealed class AlphaVantageGetCurrencyExchangeRateResponse
    {
        [JsonPropertyName("Realtime Currency Exchange Rate")]
        public AlphaVantageGetCurrencyExchangeRateResult Result { get; set; }

        public class AlphaVantageGetCurrencyExchangeRateResult
        {
            [JsonPropertyName("1. From_Currency Code")]
            public string FromCode { get; set; }

            [JsonPropertyName("2. From_Currency Name")]
            public string FromName { get; set; }

            [JsonPropertyName("3. To_Currency Code")]
            public string ToCode { get; set; }

            [JsonPropertyName("4. To_Currency Name")]
            public string ToName { get; set; }

            [JsonPropertyName("5. Exchange Rate")]
            public string Rate { get; set; }

            [JsonPropertyName("6. Last Refreshed")]
            public string LastRefreshed { get; set; }

            [JsonPropertyName("7. Time Zone")]
            public string TimeZone { get; set; }

            [JsonPropertyName("8. Bid Price")]
            public string BidPrice { get; set; }

            [JsonPropertyName("9. Ask Price")]
            public string AskPrice { get; set; }
        }
    }
}
