using Core.Pagination;

namespace Core.Tests.Pagination
{
    public class FluentPagingTests
    {
        [Fact]
        public void Paging_FromRequest_ShouldReturnsExpectedPageAndHasNext()
        {
            var index = 2;
            var size = 2;
            var request = new PagingRequest(index, size);
            var data = DummyData();
            var response = FluentPaging.From(request).Paging(data);

            for (var i = 0; i < size; i++)
            {
                Assert.Equal(data[index + i].Id, response.Data.ElementAt(i).Id);
            }
            Assert.True(response.HasNext);
        }

        [Fact]
        public void Paging_AllRequest_ShouldReturnsAllItems()
        {
            var request = PagingRequest.All();
            var data = DummyData();
            var response = FluentPaging.From(request).Paging(data);

            Assert.Equal(response.Data.Count(), data.Count);
        }

        [Fact]
        public void PagingRequest_WithInvalidParameters_ShouldThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PagingRequest(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new PagingRequest(1, 0));
        }

        [Fact]
        public void Paging_LastPage_ShouldHaveNoNext()
        {
            var data = DummyData();
            var request = new PagingRequest(4, 2); // page 4 of 4 with size 2, 8 items
            var response = FluentPaging.From(request).Paging(data);

            Assert.False(response.HasNext);
            Assert.Equal(2, response.Data.Count());
        }

        [Fact]
        public void Result_ShouldReturnAllDataWithoutPaging()
        {
            var request = new PagingRequest(1, 3);
            var data = DummyData();
            var response = FluentPaging.From(request).Result(data);

            Assert.Equal(data.Count, response.Data.Count());
        }

        private static List<PagingDummyModel> DummyData() => new()
        {
            new(1, "1"),
            new(2, "2"),
            new(3, "3"),
            new(4, "4"),
            new(5, "5"),
            new(6, "6"),
            new(7, "7"),
            new(8, "8"),
        };
        
    }

    public class PagingDummyModel(int id, string name)
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public record DummyPagingRequest(int? MaxId, int PageIndex, int PageSize) : PagingRequest(PageIndex, PageSize)
    { }
}