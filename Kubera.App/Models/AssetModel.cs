using System;

namespace Kubera.App.Models
{
    public class AssetModel : CurrencyModel
    {
        public virtual Guid Id { get; set; }

        public virtual Guid GroupId { get; set; }

        public virtual string Icon { get; set; }
    }
}
