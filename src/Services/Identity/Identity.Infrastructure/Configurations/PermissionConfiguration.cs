using Identity.Domain.Entities.UserAggrigate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations
{
    internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(nameof(Permission));

            builder.Property(x => x.Name).HasMaxLength(48);

            builder.HasKey(x => x.Id);
        }
    }
}
