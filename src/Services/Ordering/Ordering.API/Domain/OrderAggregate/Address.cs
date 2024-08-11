using Core.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.API.Domain.OrderAggregate
{
    [ComplexType]
    public class Address : ValueObject
    {
        [MaxLength(255)]
        public string Country { get; private set; }
        [MaxLength(255)]
        public string City { get; private set; }
        [MaxLength(255)]
        public string District { get; private set; }
        [MaxLength(255)]
        public string Street { get; private set; }

        protected Address() { }

        public Address(string country, string city, string district, string street)
        {
            Country = country;
            City = city;
            District = district;
            Street = street;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return City;
            yield return District;
            yield return Street;
        }
    }
}
