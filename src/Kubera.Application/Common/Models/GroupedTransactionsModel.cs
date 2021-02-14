namespace Kubera.Application.Common.Models
{
    public class GroupedTransactionsModel
    {
        public AssetModel Asset { get; set; }

        public decimal Amount { get; set; }

        public decimal TotalBought { get; set; }

        public decimal ActualValue { get; set; }
    }
}