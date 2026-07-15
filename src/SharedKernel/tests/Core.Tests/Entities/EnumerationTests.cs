using Core.Entities;

namespace Core.Tests.Entities
{
    public class Color : Enumeration<Color>
    {
        public static readonly Color Red = new(1, "Red");
        public static readonly Color Green = new(2, "Green");
        public static readonly Color Blue = new(3, "Blue");

        public Color(int id, string name) : base(id, name) { }
    }

    public class EnumerationTests
    {
        [Fact]
        public void GetValues_ShouldReturnsAllDeclaredEnumerations()
        {
            var values = Color.GetValues();

            Assert.Equal(3, values.Count);
            Assert.Contains(Color.Red, values);
            Assert.Contains(Color.Green, values);
            Assert.Contains(Color.Blue, values);
        }

        [Fact]
        public void FromValue_WithExistingId_ShouldReturnCorrectEnumeration()
        {
            var result = Color.FromValue(1);

            Assert.Equal(Color.Red, result);
        }

        [Fact]
        public void FromValue_WithNonExistingId_ShouldReturnNull()
        {
            var result = Color.FromValue(99);

            Assert.Null(result);
        }

        [Fact]
        public void FromName_WithExistingName_ShouldReturnCorrectEnumeration()
        {
            var result = Color.FromName("Green");

            Assert.Equal(Color.Green, result);
        }

        [Fact]
        public void FromName_WithNonExistingName_ShouldReturnNull()
        {
            var result = Color.FromName("Purple");

            Assert.Null(result);
        }

        [Fact]
        public void Equals_SameEnumeration_ShouldReturnTrue()
        {
            Assert.Equal(Color.Red, Color.FromValue(1));
        }

        [Fact]
        public void Equals_DifferentEnumeration_ShouldReturnFalse()
        {
            Assert.NotEqual(Color.Red, Color.Blue);
        }

        [Fact]
        public void Equals_Null_ShouldReturnFalse()
        {
            Assert.False(Color.Red.Equals(null));
        }

        [Fact]
        public void GetHashCode_SameId_ShouldReturnSameHash()
        {
            Assert.Equal(Color.Red.GetHashCode(), Color.FromValue(1)!.GetHashCode());
        }

        [Fact]
        public void ToString_ShouldReturnName()
        {
            Assert.Equal("Blue", Color.Blue.ToString());
        }
    }
}
