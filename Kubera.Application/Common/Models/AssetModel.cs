using System;

namespace Kubera.Application.Common.Models
{
    public class AssetModel : CurrencyModel
    {
        public virtual Guid GroupId { get; set; }

        public virtual string Icon { get; set; }

        public virtual GroupModel Group { get; set; }
    }
}
