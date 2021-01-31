using Kubera.General.Entities;
using System;

namespace Kubera.Data.Entities
{
    public class Transaction : Entity, ISoftDeletable
    {
        public virtual Guid AssetId { get; set; }

        public virtual string OwnerId { get; set; }

        public virtual string Wallet { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual Guid CurrencyId { get; set; }

        public virtual decimal Rate { get; set; }

        public virtual decimal? Fee { get; set; }

        public virtual Guid? FeeCurrencyId { get; set; }

        public bool Deleted { get; set; }


        public virtual Asset Asset { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual Currency Currency { get; set; }

        public virtual Currency FeeCurrency { get; set; }
    }
}
