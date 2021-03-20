﻿using System;
using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("Id: {Id}, Asset: {Asset.Name}, Wallet: {Wallet}, CreatedAt: {CreatedAt}, Date: {Date}, Amount: {Amount}, Currency: {Currency.Name}, Rate: {Rate}, Fee: {Fee}")]
    public class TransactionModel
    {
        public Guid Id { get; set; }

        public virtual Guid AssetId { get; set; }

        public virtual string Wallet { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual Guid CurrencyId { get; set; }

        public virtual decimal Rate { get; set; }

        public virtual decimal? Fee { get; set; }

        public virtual Guid? FeeCurrencyId { get; set; }

        public virtual AssetModel Asset { get; set; }

        public virtual GroupModel Group => Asset?.Group;

        public virtual CurrencyModel Currency { get; set; }

        public virtual CurrencyModel FeeCurrency { get; set; }
    }
}
