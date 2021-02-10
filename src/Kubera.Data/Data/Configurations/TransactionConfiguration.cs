using Kubera.Data.Entities;
using Kubera.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kubera.Data.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Wallet).IsRequired().HasMaxLength(Constraints.Length.Transaction.Wallet);
            builder.Property(t => t.CreatedAt).IsRequired().HasDefaultValueSql(Constraints.SQL.Default.GetUtcDate);
            builder.Property(t => t.Amount).IsRequired().HasPrecision(38, 8);
            builder.Property(t => t.Rate).IsRequired().HasPrecision(38, 8);
            builder.Property(t => t.Fee).IsRequired(false).HasPrecision(38, 8);
            builder.Property(t => t.Deleted).IsRequired(false).HasDefaultValue(false);

            builder.HasOne(t => t.Asset).WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AssetId);

            builder.HasOne(t => t.Owner).WithMany(au => au.Transactions)
                .HasForeignKey(t => t.OwnerId);

            builder.HasOne(t => t.Currency).WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CurrencyId);

            builder.HasOne(t => t.FeeCurrency).WithMany(c => c.FeeTransactions)
                .HasForeignKey(t => t.FeeCurrencyId)
                .IsRequired(false);
        }
    }
}
