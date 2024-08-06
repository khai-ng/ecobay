using Core.EntityFramework.ServiceDefault;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.API.Domain.OrderAgrregate
{
    public class OrderItem: Entity
    {
        [ForeignKey(nameof(Order))]
        public Ulid OrderId { get; private set; }
        [MaxLength(24)]
        public string ProductId { get; private set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal Price { get; private set; }
        public int Unit { get; private set; }

    }
}
