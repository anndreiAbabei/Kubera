using Kubera.General.Entities;
using System;
using System.Diagnostics;

namespace Kubera.Data.Entities
{
    [DebuggerDisplay("Id: {Id}, Asset: {Asset.Name}, Amount: {Amount}, Rate: {Rate}, Currency: {Currency.Name}, Owner: {Owner.Email}")]
    public class Transaction : Entity, ISoftDeletable, IDateEntity
    {
        public virtual Guid AssetId { get; set; }

        public virtual string OwnerId { get; set; }

        public virtual string Wallet { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime Date { get; set; }

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
