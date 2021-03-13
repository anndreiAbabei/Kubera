using System;
using System.Diagnostics;
using Kubera.General.Models;

namespace Kubera.Business.Models.AlphaVantage
{
    [DebuggerDisplay("From: {From}, To: {To}, Timestamp: {Timestamp}, Rate: {Rate}")]
    public sealed class AlphaVantageForexServiceResponse : IForexServiceResponse
    {
        public string From { get; init; }

        public string To { get; init; }

        public DateTime Timestamp { get; init; }

        public decimal Rate { get; init; }
    }
}
