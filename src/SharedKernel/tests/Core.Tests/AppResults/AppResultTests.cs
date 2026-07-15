using Core.AppResults;

namespace Core.Tests.AppResults
{
    public class AppResultTests
    {
        // --- AppResult (void) ---

        [Fact]
        public void Success_Void_ShouldBeSuccessWithOkStatus()
        {
            var result = AppResult.Success();
            Assert.True(result.IsSuccess);
            Assert.Equal(AppStatusCode.Ok, result.Status);
        }

        [Fact]
        public void Success_WithMessage_ShouldBeSuccessAndCarryData()
        {
            var result = AppResult.Success("success");
            Assert.True(result.IsSuccess);
            Assert.Equal(AppStatusCode.Ok, result.Status);
        }

        [Fact]
        public void Error_SingleMessage_ShouldBeFailureWithErrorStatus()
        {
            var result = AppResult.Error("error");
            Assert.False(result.IsSuccess);
            Assert.Equal(AppStatusCode.Error, result.Status);
            Assert.Single(result.Errors!);
        }

        [Fact]
        public void Error_MultipleMessages_ShouldContainAllErrors()
        {
            var result = AppResult.Error("e1", "e2", "e3");
            Assert.False(result.IsSuccess);
            Assert.Equal(3, result.Errors!.Count());
        }

        [Fact]
        public void Invalid_WithSingleErrorDetail_ShouldContainOneError()
        {
            var result = AppResult.Invalid(new ErrorDetail("test"));
            Assert.False(result.IsSuccess);
            Assert.Equal(AppStatusCode.Invalid, result.Status);
            Assert.Equal(1, result.Errors?.Count());
        }

        [Fact]
        public void Invalid_WithMultipleErrorDetails_ShouldContainAllErrors()
        {
            var result = AppResult.Invalid(new ErrorDetail("f1", "msg1"), new ErrorDetail("f2", "msg2"));
            Assert.False(result.IsSuccess);
            Assert.Equal(2, result.Errors!.Count());
        }

        [Fact]
        public void Invalid_WithIEnumerableErrors_ShouldContainAllErrors()
        {
            IEnumerable<ErrorDetail> errors = [new ErrorDetail("a"), new ErrorDetail("b")];
            var result = AppResult.Invalid(errors);
            Assert.False(result.IsSuccess);
            Assert.Equal(2, result.Errors!.Count());
        }

        [Fact]
        public void NotFound_Void_ShouldHaveNotFoundStatus()
        {
            var result = AppResult.NotFound();
            Assert.False(result.IsSuccess);
            Assert.Equal(AppStatusCode.NotFound, result.Status);
        }

        [Fact]
        public void NotFound_WithMessage_ShouldCarryMessage()
        {
            var result = AppResult.NotFound("item not found");
            Assert.False(result.IsSuccess);
            Assert.Equal("item not found", result.Message);
        }

        [Fact]
        public void NotFound_WithErrorMessages_ShouldContainErrors()
        {
            var result = AppResult.NotFound("e1", "e2");
            Assert.Equal(2, result.Errors!.Count());
        }

        [Fact]
        public void Forbidden_ShouldHaveForbiddenStatus()
        {
            var result = AppResult.Forbidden();
            Assert.False(result.IsSuccess);
            Assert.Equal(AppStatusCode.Forbidden, result.Status);
        }

        [Fact]
        public void Unauthorized_ShouldHaveUnauthorizedStatus()
        {
            var result = AppResult.Unauthorized();
            Assert.False(result.IsSuccess);
            Assert.Equal(AppStatusCode.Unauthorized, result.Status);
        }

        // --- AppResult<T> ---

        [Fact]
        public void GenericSuccess_ShouldCarryDataAndBeSuccess()
        {
            var result = AppResult.Success(42);
            Assert.True(result.IsSuccess);
            Assert.Equal(42, result.Data);
        }

        [Fact]
        public void GenericError_ShouldBeFailureWithErrorStatus()
        {
            var result = AppResult<int>.Error("oops");
            Assert.False(result.IsSuccess);
            Assert.Equal(AppStatusCode.Error, result.Status);
        }

        [Fact]
        public void GenericNotFound_ShouldHaveNotFoundStatus()
        {
            var result = AppResult<string>.NotFound();
            Assert.False(result.IsSuccess);
            Assert.Equal(AppStatusCode.NotFound, result.Status);
        }

        [Fact]
        public void GenericNotFound_WithMessage_ShouldCarryMessage()
        {
            var result = AppResult<string>.NotFound("not here");
            Assert.Equal("not here", result.Message);
        }

        [Fact]
        public void GenericForbidden_ShouldHaveForbiddenStatus()
        {
            var result = AppResult<string>.Forbidden();
            Assert.Equal(AppStatusCode.Forbidden, result.Status);
        }

        [Fact]
        public void GenericUnauthorized_ShouldHaveUnauthorizedStatus()
        {
            var result = AppResult<string>.Unauthorized();
            Assert.Equal(AppStatusCode.Unauthorized, result.Status);
        }

        [Fact]
        public void GenericInvalid_WithMultipleErrors_ShouldContainAllErrors()
        {
            var result = AppResult<string>.Invalid(new ErrorDetail("f1", "m1"), new ErrorDetail("f2", "m2"));
            Assert.Equal(2, result.Errors!.Count());
        }

        // --- Implicit conversions ---

        [Fact]
        public void ImplicitConversion_FromValueToGenericResult_ShouldBeSuccess()
        {
            AppResult<int> result = 99;
            Assert.True(result.IsSuccess);
            Assert.Equal(99, result.Data);
        }

        [Fact]
        public void ImplicitConversion_FromGenericResultToValue_ShouldReturnData()
        {
            var result = AppResult.Success(7);
            int value = result;
            Assert.Equal(7, value);
        }

        [Fact]
        public void ImplicitConversion_FromVoidResultToGenericResult_ShouldPreserveStatus()
        {
            AppResult voidResult = AppResult.Error("fail");
            AppResult<int> typed = voidResult;
            Assert.False(typed.IsSuccess);
            Assert.Equal(AppStatusCode.Error, typed.Status);
        }

        // --- ErrorDetail ---

        [Fact]
        public void ErrorDetail_WithMessageOnly_ShouldHaveDefaultSeverityError()
        {
            var detail = new ErrorDetail("some error");
            Assert.Equal("some error", detail.Message);
            Assert.Null(detail.Name);
            Assert.Equal(ValidationSeverity.Error, detail.Severity);
        }

        [Fact]
        public void ErrorDetail_WithNameAndMessage_ShouldSetBothProperties()
        {
            var detail = new ErrorDetail("Field", "is required");
            Assert.Equal("Field", detail.Name);
            Assert.Equal("is required", detail.Message);
        }

        [Fact]
        public void ErrorDetail_WithSeverity_ShouldSetSeverity()
        {
            var detail = new ErrorDetail("Field", "warning msg", ValidationSeverity.Warning);
            Assert.Equal(ValidationSeverity.Warning, detail.Severity);
        }
    }
}
