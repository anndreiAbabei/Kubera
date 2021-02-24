namespace Kubera.Application.Common.Models
{
    public class GroupTotalModel : GroupModel
    {
        public decimal SumAmount { get; set; }

        public decimal Total { get; set; }

        public decimal? TotalNow { get; set; }

        public float Increase { get; set; }
    }
}
