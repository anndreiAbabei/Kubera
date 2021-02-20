namespace Kubera.Application.Common.Models
{
    public class AssetTotalModel : AssetModel
    {
        public decimal SumAmount { get; set; }

        public decimal Total { get; set; }

        public decimal? TotalNow { get; set; }

        public float Increase { get; set; }
    }
}
