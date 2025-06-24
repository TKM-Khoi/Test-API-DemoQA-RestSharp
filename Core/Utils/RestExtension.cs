using System.Net;

using FluentAssertions;

using Newtonsoft.Json;

using RestSharp;

using static Core.Utils.JsonFileUtils;

namespace Test.Core.Extensions
{
    public static class RestExtension
    {
        public static async Task<RestResponse> VerifySchema(this RestResponse response, string pathFile)
        {
            var schema = await NJsonSchema.JsonSchema.FromJsonAsync(JsonFileUltils.ReadJsonFile(pathFile));
            schema.Validate(response.Content).Should().BeEmpty();
            return response;
        }
        public static dynamic ConvertToDynamicObject(this RestResponse response)
        {
            return (dynamic)JsonConvert.DeserializeObject(response.Content);
        }
        public static RestResponse VerifyStatusCodeOk(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            return response;
        }
        public static RestResponse VerifyStatusCodeCreated(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            return response;
        }
        public static RestResponse VerifyStatusCodeNoContent(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            return response;
        }
        public static RestResponse VerifyStatusCodeBadRequest(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            return response;
        }
        public static RestResponse VerifyStatusCodeUnauthorized(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            return response;
        }
        public static RestResponse VerifyStatusCodeForbidden(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            return response;
        }
        public static RestResponse VerifyStatusCodeInternalServerError(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            return response;
        }

    }
}