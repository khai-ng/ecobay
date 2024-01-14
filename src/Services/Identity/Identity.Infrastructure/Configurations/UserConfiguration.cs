using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");
            builder.Property(x => x.UserName).HasMaxLength(128);
            builder.Property(x => x.Name).HasMaxLength(128);
            builder.Property(x => x.Email).HasMaxLength(128);
            builder.Property(x => x.Phone).HasMaxLength(48);

            builder.HasMany(x => x.Roles)
                .WithMany()
                .UsingEntity<UserRole>(
                l => l.HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId),
                r => r.HasOne<User>().WithMany().HasForeignKey(e => e.UserId));

            builder.HasMany(x => x.Permissions)
                .WithMany()
                .UsingEntity<UserPermission>(
                l => l.HasOne<Permission>().WithMany().HasForeignKey(e => e.PermissionId),
                r => r.HasOne<User>().WithMany().HasForeignKey(e => e.UserId));
        }
    }
}
