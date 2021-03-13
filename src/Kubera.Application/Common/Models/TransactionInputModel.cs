using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("AssetId: {AssetId}, Wallet: {Wallet}, CreatedAt: {CreatedAt}, Amount: {Amount}, CurrencyId: {CurrencyId}, Rate: {Rate}, Fee: {Fee}")]
    public class TransactionInputModel
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

    [DebuggerDisplay("AssetId: {AssetId}, Wallet: {Wallet}, CreatedAt: {CreatedAt}, Amount: {Amount}, CurrencyId: {CurrencyId}, Rate: {Rate}, Fee: {Fee}")]
    public class TransactionUpdateModel : TransactionInputModel { }
}
