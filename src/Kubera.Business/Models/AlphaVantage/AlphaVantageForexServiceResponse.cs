using System;
using Kubera.General.Models;

namespace Kubera.Business.Models.AlphaVantage
{
    public sealed class AlphaVantageForexServiceResponse : IForexServiceResponse
    {
        public string From { get; init; }

        public string To { get; init; }

        public DateTime Timestamp { get; init; }

        public decimal Rate { get; init; }
    }
}
