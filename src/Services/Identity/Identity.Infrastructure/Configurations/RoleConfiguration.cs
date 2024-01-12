using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role));
            builder.HasKey(x => x.Id);

			builder.Property(x => x.Id)
                .ValueGeneratedOnAdd(); ;

			builder.HasMany(x => x.Users)
                .WithMany()
                .UsingEntity<UserRole>(
                l => l.HasOne<User>().WithMany().HasForeignKey(e => e.UserId),
                r => r.HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId));

            builder.HasMany(x => x.Permissions)
                .WithMany()
                .UsingEntity<RolePermission>(
                l => l.HasOne<Permission>().WithMany().HasForeignKey(e => e.PermissionId),
                r => r.HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId));
            builder.HasData(Role.GetValues());
        }
    }
}
