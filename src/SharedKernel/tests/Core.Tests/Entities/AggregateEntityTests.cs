using Core.Entities;
using Core.Events.DomainEvents;

namespace Core.Tests.Entities
{
    public enum OrderStatus
    {
        Initiated,
        Paid,
        Shipped,
        Completed,
    }

    public record OrderInitiated(string Description) : DomainEvent<Guid>(Guid.CreateVersion7());

    public record OrderPaid(Guid Id): DomainEvent<Guid>(Id);

    public class Order : AggregateRoot<Guid>
    {
        public OrderStatus Status { get; set; }
        public string Description { get; set; }

        public Order(string description) {
            Description = description;
            Enqueue(new OrderInitiated(description));
        } 

        public void SetPaid()
        {
            Enqueue(new OrderPaid(Id));
        }

        public override void Apply(IDomainEvent<Guid> @event)
        {
            switch (@event)
            {
                case OrderInitiated orderInitiated:
                    Id = orderInitiated.AggregateId;
                    Status = OrderStatus.Initiated;
                    Description = orderInitiated.Description;
                    break;
                case OrderPaid _:
                    Status = OrderStatus.Paid;
                    break;
            }
        }
    }

    public class AggregateEntityTests
    {
        [Fact]
        public void Order_WhenCreated_ShouldEnqueuesOrderInitiatedEventAndIncrementsVersion()
        {
            var fakeOrder = new Order("test");
            Assert.Single(fakeOrder.Events);
            Assert.Equal(1, fakeOrder.Version);
        }

        [Fact]
        public void Order_WhenSetPaid_ShouldSetsStatusToPaid()
        {
            var fakeDesc = "test";
            var fakeOrder = new Order(fakeDesc);        
            Assert.Equal(OrderStatus.Initiated, fakeOrder.Status);

            fakeOrder.SetPaid();
            Assert.Equal(fakeDesc, fakeOrder.Description);
            Assert.Equal(OrderStatus.Paid, fakeOrder.Status);
        }

        [Fact]
        public void Order_MultipleEvents_ShouldIncrementVersionForEachEvent()
        {
            var fakeOrder = new Order("test");
            fakeOrder.SetPaid();

            Assert.Equal(2, fakeOrder.Events.Count);
            Assert.Equal(2, fakeOrder.Version);
        }

        [Fact]
        public void ClearEvents_ShouldEmptyEventsCollection()
        {
            var fakeOrder = new Order("test");
            fakeOrder.SetPaid();
            Assert.Equal(2, fakeOrder.Events.Count);

            fakeOrder.ClearEvents();

            Assert.Empty(fakeOrder.Events);
        }

        [Fact]
        public void ClearEvents_ShouldNotResetVersion()
        {
            var fakeOrder = new Order("test");
            fakeOrder.SetPaid();
            fakeOrder.ClearEvents();

            Assert.Equal(2, fakeOrder.Version);
        }
    }
}
