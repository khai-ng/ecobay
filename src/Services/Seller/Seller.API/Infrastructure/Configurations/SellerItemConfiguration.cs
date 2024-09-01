using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seller.API.Domain.SellerAggregate;

namespace Seller.API.Infrastructure.Configurations
{
    public class SellerItemConfiguration : IEntityTypeConfiguration<SellerItem>
    {
        public void Configure(EntityTypeBuilder<SellerItem> builder)
        {
            builder.ToTable(nameof(SellerItem));

            builder.Property(x => x.Name).HasMaxLength(255);
        }
    }
}
