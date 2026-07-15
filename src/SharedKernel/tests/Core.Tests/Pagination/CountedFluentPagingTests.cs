using Core.Pagination;

namespace Core.Tests.Pagination
{
    public class CountedFluentPagingTests
    {
        [Fact]
        public void From_WithRequest_ShouldCreateInstance()
        {
            var request = new PagingRequest(1, 5);
            var paging = CountedFluentPaging.From(request);

            Assert.NotNull(paging);
            Assert.Equal(1, paging.PageIndex);
            Assert.Equal(5, paging.PageSize);
        }

        [Fact]
        public void SetTotal_ShouldCalculatePageCount()
        {
            var request = new PagingRequest(1, 3);
            var paging = CountedFluentPaging.From(request).SetTotal(9);

            Assert.Equal(9, paging.TotalCount);
            Assert.Equal(3, paging.PageCount);
        }

        [Fact]
        public void SetTotal_WithNonEvenDivision_ShouldCeilPageCount()
        {
            var request = new PagingRequest(1, 3);
            var paging = CountedFluentPaging.From(request).SetTotal(10);

            Assert.Equal(10, paging.TotalCount);
            Assert.Equal(4, paging.PageCount);
        }

        [Fact]
        public void Paging_FromRequest_ShouldReturnPagedDataWithTotalCount()
        {
            var index = 2;
            var size = 2;
            var request = new PagingRequest(index, size);
            var data = DummyData();
            var response = CountedFluentPaging.From(request).Paging(data);

            for (var i = 0; i < size; i++)
            {
                Assert.Equal(data[index + i].Id, response.Data.ElementAt(i).Id);
            }
            Assert.Equal(data.Count, response.TotalCount);
            Assert.True(response.HasNext);
        }

        [Fact]
        public void Paging_AllRequest_ShouldReturnAllItemsWithTotalCount()
        {
            var request = PagingRequest.All();
            var data = DummyData();
            var response = CountedFluentPaging.From(request).Paging(data);

            Assert.Equal(data.Count, response.Data.Count());
            Assert.Equal(data.Count, response.TotalCount);
            Assert.Equal(1, response.PageCount);
        }

        [Fact]
        public void Result_ShouldReturnDataWithoutPaging()
        {
            var request = new PagingRequest(1, 3);
            var data = DummyData();
            var paging = CountedFluentPaging.From(request);
            paging.SetTotal(data.Count);
            var response = paging.Result(data);

            Assert.Equal(data.Count, response.Data.Count());
            Assert.Equal(data.Count, response.TotalCount);
        }

        [Fact]
        public void Counted_FromFluentPaging_ShouldConvertToCountedFluentPaging()
        {
            var request = new PagingRequest(1, 3);
            var counted = FluentPaging.From(request).Counted();

            Assert.NotNull(counted);
            Assert.Equal(1, counted.PageIndex);
            Assert.Equal(3, counted.PageSize);
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
}
