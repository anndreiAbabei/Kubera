using System;
namespace Kubera.General.Models
{
    public interface IForexServiceResponse
    {
        string From { get; }
        string To { get; }
        DateTime Timestamp { get; }
        decimal Rate { get; }
    }
}
