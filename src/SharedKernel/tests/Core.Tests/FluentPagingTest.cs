using Core.Pagination;

namespace Core.Tests
{
    public class FluentPagingTest
    {
        [Fact]
        public void paging_success()
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
        public void paging_getall_success()
        {
            var request = PagingRequest.All();
            var data = DummyData();
            var response = FluentPaging.From(request).Paging(data);

            Assert.Equal(response.Data.Count(), data.Count);
        }

        [Fact]
        public void invalid_paging_request()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PagingRequest(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new PagingRequest(1, 0));
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