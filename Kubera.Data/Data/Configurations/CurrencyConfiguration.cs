using Kubera.Data.Entities;
using Kubera.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kubera.Data.Data.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code).IsRequired().HasMaxLength(Constraints.Length.Currency.Code);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(Constraints.Length.Currency.Name);
            builder.Property(c => c.Symbol).IsRequired().HasMaxLength(Constraints.Length.Currency.Symbol);
            builder.Property(c => c.Order).IsRequired().ValueGeneratedOnAdd();
            builder.Property(c => c.Deleted).IsRequired(false).HasDefaultValue(false);
        }
    }
}
