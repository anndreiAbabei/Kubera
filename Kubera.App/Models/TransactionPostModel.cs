using System;
using System.ComponentModel.DataAnnotations;

namespace Kubera.App.Models
{
    public class TransactionPostModel
    {
        [Required]
        public virtual Guid AssetId { get; set; }

        [Required]
        public virtual string Wallet { get; set; }

        [Required]
        public virtual DateTime CreatedAt { get; set; }

        [Required]
        public virtual decimal Amount { get; set; }

        [Required]
        public virtual Guid CurrencyId { get; set; }

        [Required]
        public virtual decimal Rate { get; set; }

        public virtual decimal? Fee { get; set; }

        public virtual Guid? FeeCurrencyId { get; set; }
    }

    public class TransactionPutModel : TransactionPostModel { }
}
