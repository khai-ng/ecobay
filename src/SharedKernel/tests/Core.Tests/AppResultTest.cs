using Core.AppResults;

namespace Core.Tests
{
    public class AppResultTest
    {
        [Fact]
        public void Success_WhenCalled_ShouldBeSuccess()
        {
            var result = AppResult.Success("success");
            Assert.True(result.IsSuccess);
        }

        [Fact] 
        public void Error_WhenCalled_ShouldBeFailure()
        {
            var result = AppResult.Error("error");
            Assert.True(!result.IsSuccess);
        }

        [Fact]
        public void Invalid_WithErrorDetail_ShouldContainErrors()
        {
            var result = AppResult.Invalid(new ErrorDetail("test"));
            Assert.True(!result.IsSuccess);
            Assert.Equal(result.Errors?.Count(), 1);
        }
    }
}
