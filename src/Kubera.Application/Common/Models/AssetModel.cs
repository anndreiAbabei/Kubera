using System;
using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("Code: {Code}, Name: {Name}, Symbol: {Symbol}, Order: {Order}")]
    public class AssetModel : CurrencyModel
    {
        public virtual Guid GroupId { get; set; }

        public virtual string Icon { get; set; }

        public virtual GroupModel Group { get; set; }
    }
}
