using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("Group: {Name}, SumAmount: {SumAmount}, Total: {Total}, TotalNow: {TotalNow}, Increase: {Increase}")]
    public class GroupTotalModel : GroupModel
    {
        public decimal SumAmount { get; set; }

        public decimal Total { get; set; }

        public decimal? TotalNow { get; set; }

        public float Increase { get; set; }
    }
}
