using System;
using System.Diagnostics;
using Kubera.General.Models;

namespace Kubera.Business.Models.AlphaVantage
{
    [DebuggerDisplay("Stock: {Stock}, To: {To}, Rate: {Rate}")]
    public sealed class AlphaVantageStockServiceResponse : IStockServiceResponse
    {
        public string Stock { get; init; }

        public string To { get; init; }

        public DateTime Timestamp { get; init; }

        public decimal Rate { get; init; }
    }
}
