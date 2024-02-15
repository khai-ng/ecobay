using FastEndpoints;
using Kernel.Result;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace ServiceDefaults
{
    public static class FastEndpointExtensions
    {
        /// <summary>
        /// Handle validation failure (400 code), and serialize dto
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Config CommonResponseConfigs(this Config config)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            config.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
            {
                return new HttpErrorResult((HttpStatusCode)statusCode,
                    failures.Select(e => new ErrorDetail(e.PropertyName, e.ErrorMessage)));
            };

            config.Serializer.ResponseSerializer = (reposnse, dto, cType, jsonContext, ct) =>
            {
                reposnse.ContentType = cType;
                return reposnse.WriteAsync(JsonSerializer.Serialize(dto, jsonSerializerOptions), ct);
            };

            return config;
        }
    }
}
