using System;
namespace Kubera.General.Models
{
    public interface IStockServiceResponse
    {
        string Stock { get; }
        string To { get; }
        DateTime Timestamp { get; }
        decimal Rate { get; }
    }
}
