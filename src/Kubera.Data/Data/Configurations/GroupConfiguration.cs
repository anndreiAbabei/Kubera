using Kubera.Data.Entities;
using Kubera.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kubera.Data.Data.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Code).IsRequired().HasMaxLength(Constraints.Length.Group.Code);
            builder.Property(g => g.Name).IsRequired().HasMaxLength(Constraints.Length.Group.Name);
            builder.Property(g => g.CreatedAt).IsRequired().HasDefaultValueSql(Constraints.SQL.Default.GetUtcDate);
            builder.Property(g => g.Deleted).IsRequired(false).HasDefaultValue(false);

            builder.HasOne(g => g.Owner).WithMany(au => au.Groups)
                .HasForeignKey(g => g.OwnerId)
                .IsRequired(false);

            builder.HasMany(g => g.Assets).WithOne(a => a.Group)
                .HasForeignKey(a => a.GroupId);
        }
    }
}
