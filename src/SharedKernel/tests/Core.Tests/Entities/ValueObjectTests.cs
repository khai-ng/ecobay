using Core.Entities;

namespace Core.Tests.Entities
{
    public class Money : ValueObject
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }

    public class ValueObjectTests
    {
        [Fact]
        public void Equals_SameComponents_ReturnsTrue()
        {
            var a = new Money(10m, "USD");
            var b = new Money(10m, "USD");

            Assert.Equal(a, b);
        }

        [Fact]
        public void Equals_DifferentAmount_ReturnsFalse()
        {
            var a = new Money(10m, "USD");
            var b = new Money(20m, "USD");

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Equals_DifferentCurrency_ReturnsFalse()
        {
            var a = new Money(10m, "USD");
            var b = new Money(10m, "EUR");

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Equals_Null_ReturnsFalse()
        {
            var a = new Money(10m, "USD");

            Assert.False(a.Equals(null));
        }

        [Fact]
        public void Equals_Object_SameComponents_ReturnsTrue()
        {
            var a = new Money(10m, "USD");
            object b = new Money(10m, "USD");

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_Object_NonValueObject_ReturnsFalse()
        {
            var a = new Money(10m, "USD");

            Assert.False(a.Equals("not a value object"));
        }

        [Fact]
        public void EqualityOperator_BothNull_ReturnsTrue()
        {
            Money? a = null;
            Money? b = null;

            Assert.True(a == b);
        }

        [Fact]
        public void EqualityOperator_LeftNull_ReturnsFalse()
        {
            Money? a = null;
            var b = new Money(10m, "USD");

            Assert.False(a == b);
        }

        [Fact]
        public void EqualityOperator_RightNull_ReturnsFalse()
        {
            var a = new Money(10m, "USD");
            Money? b = null;

            Assert.False(a == b);
        }

        [Fact]
        public void InequalityOperator_DifferentComponents_ReturnsTrue()
        {
            var a = new Money(10m, "USD");
            var b = new Money(20m, "USD");

            Assert.True(a != b);
        }

        [Fact]
        public void GetHashCode_SameComponents_ReturnsSameHash()
        {
            var a = new Money(10m, "USD");
            var b = new Money(10m, "USD");

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void GetHashCode_DifferentComponents_ReturnsDifferentHash()
        {
            var a = new Money(10m, "USD");
            var b = new Money(20m, "USD");

            Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}
