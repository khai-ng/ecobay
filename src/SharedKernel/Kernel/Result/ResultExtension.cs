using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace SharedKernel.Kernel.Result
{
    public static class ResultExtension
    {
        public static IResult ToHttpResult<T>(this AppResult<T>? result)
        {
            if (result is null) return Results.NoContent();

            return result.Status switch
            {
                ResultStatus.Ok => Results.Ok(result.Value),
                ResultStatus.Error => ErrorEntity(result),
                ResultStatus.Forbidden => Results.Forbid(),
                ResultStatus.Unauthorized => Results.Unauthorized(),
                ResultStatus.Invalid => Results.BadRequest(result.Errors),
                ResultStatus.NotFound => NotFoundEntity(result),
                ResultStatus.Conflict => ConflictEntity(result),
                ResultStatus.Unavailable => UnavailableEntity(result),
                _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
            };
        }


        private static IResult NotFoundEntity(IAppResult result)
        {
            var details = new StringBuilder("Next error(s) occured:");

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

                return Results.NotFound(new ProblemDetails
                {
                    Detail = details.ToString()
                });
            }
            else
            {
                return Results.NotFound();
            }
        }

        private static IResult ConflictEntity(IAppResult result)
        {
            var details = new StringBuilder("Next error(s) occured:");

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

                return Results.Conflict(new ProblemDetails
                {
                    Detail = details.ToString()
                });
            }
            else
            {
                return Results.Conflict();
            }
        }

        private static IResult ErrorEntity(IAppResult result)
        {
            var details = new StringBuilder("Next error(s) occured:");

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

                return Results.Problem(new ProblemDetails()
                {
                    Detail = details.ToString(),
                    Status = StatusCodes.Status500InternalServerError
                });
            }
            else
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private static IResult UnavailableEntity(IAppResult result)
        {
            var details = new StringBuilder("Next error(s) occured:");

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors) details.Append("* ").Append(error).AppendLine();

                return Results.Problem(new ProblemDetails
                {
                    Detail = details.ToString(),
                    Status = StatusCodes.Status503ServiceUnavailable
                });
            }
            else
            {
                return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
        }
    }
}
