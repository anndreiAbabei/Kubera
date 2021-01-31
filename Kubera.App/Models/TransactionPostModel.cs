using System;

namespace Kubera.App.Models
{
    public class TransactionPostModel
    {
        public virtual Guid AssetId { get; set; }

        public virtual string Wallet { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual Guid CurrencyId { get; set; }

        public virtual decimal Rate { get; set; }

        public virtual decimal? Fee { get; set; }

        public virtual Guid? FeeCurrencyId { get; set; }
    }

    public class TransactionPutModel : TransactionPostModel { }
}
