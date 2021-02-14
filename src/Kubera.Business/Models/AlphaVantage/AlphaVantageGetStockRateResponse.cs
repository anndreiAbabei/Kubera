using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kubera.Business.Models.AlphaVantage
{
    public class AlphaVantageGetStockRateResponse
    {
        [JsonPropertyName("Meta Data")]
        public AlphaVantageGetStockMetaData MetaData { get; set; }

        [JsonPropertyName("Time Series (5min)")]
        public IDictionary<string, AlphaVantageGetStockSerie> Series { get; set; }

        public class AlphaVantageGetStockMetaData
        {
            [JsonPropertyName("1. Information")]
            public string Information { get; set; }

            [JsonPropertyName("2. Symbol")]
            public string Code { get; set; }

            [JsonPropertyName("3. Last Refreshed")]
            public string LastRefreshed { get; set; }

            [JsonPropertyName("4. Interval")]
            public string Interval { get; set; }

            [JsonPropertyName("5. Output Size")]
            public string OutputSize { get; set; }

            [JsonPropertyName("6. Time Zone")]
            public string Timezone { get; set; }
        }

        public class AlphaVantageGetStockSerie
        {
            [JsonPropertyName("1. open")]
            public string Open { get; set; }

            [JsonPropertyName("2. high")]
            public string High { get; set; }

            [JsonPropertyName("3. low")]
            public string Low { get; set; }

            [JsonPropertyName("4. close")]
            public string Close { get; set; }

            [JsonPropertyName("5. volume")]
            public string Volume { get; set; }
        }
    }
}
