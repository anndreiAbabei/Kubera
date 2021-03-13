using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("Asset: {Asset.Name}, Amount: {Amount}, TotalBought: {TotalBought}, ActualValue: {ActualValue}")]
    public class GroupedTransactionsModel
    {
        public AssetModel Asset { get; set; }

        public decimal Amount { get; set; }

        public decimal TotalBought { get; set; }

        public decimal? ActualValue { get; set; }
    }
}