using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("Code: {Code}, Name: {Name}, SumAmount: {SumAmount}, Total: {Total}, TotalNow: {TotalNow}, Increase: {Increase}")]
    public class AssetTotalModel : AssetModel
    {
        public decimal SumAmount { get; set; }

        public decimal Total { get; set; }

        public decimal? TotalNow { get; set; }

        public float Increase { get; set; }
    }
}
