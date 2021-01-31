using Kubera.Data.Entities;
using Kubera.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kubera.Data.Data.Configurations
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Code).IsRequired().HasMaxLength(Constraints.Length.Asset.Code);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(Constraints.Length.Asset.Name);
            builder.Property(a => a.Symbol).IsRequired().HasMaxLength(Constraints.Length.Asset.Symbol);
            builder.Property(a => a.Order).IsRequired().ValueGeneratedOnAdd();
            builder.Property(a => a.Icon).IsRequired(false);
            builder.Property(a => a.CreatedAt).IsRequired().HasDefaultValueSql(Constraints.SQL.Default.GetUtcDate);
            builder.Property(a => a.Deleted).IsRequired(false).HasDefaultValue(false);

            builder.HasOne(a => a.Group).WithMany(g => g.Assets)
                .HasForeignKey(a => a.GroupId);

            builder.HasOne(a => a.Owner).WithMany(au => au.Assets)
                .HasForeignKey(a => a.OwnerId)
                .IsRequired(false);

            builder.HasMany(a => a.Transactions).WithOne(t => t.Asset)
                .HasForeignKey(a => a.AssetId);
        }
    }
}
