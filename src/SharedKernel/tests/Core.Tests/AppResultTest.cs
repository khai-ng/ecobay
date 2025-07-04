using Core.AppResults;

namespace Core.Tests
{
    public class AppResultTest
    {
        [Fact]
        public void result_success()
        {
            var result = AppResult.Success("success");
            Assert.True(result.IsSuccess);
        }

        [Fact] 
        public void result_fail()
        {
            var result = AppResult.Error("error");
            Assert.True(!result.IsSuccess);
        }

        [Fact]
        public void result_error_detail()
        {
            var result = AppResult.Invalid(new ErrorDetail("test"));
            Assert.True(!result.IsSuccess);
            Assert.Equal(result.Errors?.Count(), 1);
        }
    }
}
