using System.Diagnostics;
using System.Text.Json.Serialization;
using Kubera.General.Models;

namespace Kubera.Business.Models.AlphaVantage
{
    [DebuggerDisplay("Code: {Code}, Name: {Name}, Exchange: {Exchange}, Currency: {Currency}, Country: {Country}")]
    public class AlphaVantageStockCompanyResponse : IStockCompanyResponse
    {
        [JsonPropertyName("Symbol")]
        public string Code { get; init; }

        public string Name { get; init; }

        public string Exchange { get; init; }

        public string Currency { get; init; }

        public string Country { get; init; }
    }
}
