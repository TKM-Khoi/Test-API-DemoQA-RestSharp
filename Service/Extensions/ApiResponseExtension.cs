using FluentAssertions;
using FluentAssertions.Execution;

using RestSharp;

using Test.Core.Extensions;

namespace Service.Extensions
{
    public static class ApiResponseExtension
    {
        public static RestResponse VerifyErrorPayload(this RestResponse response, int errorCode, string errorMsg)
        {
            var dynamicRes = response.ConvertToDynamicObject();
            using (new AssertionScope())
            {
                ((int)dynamicRes["code"]).Should().Be(errorCode);
                ((string)dynamicRes["message"]).Should().Be(errorMsg);
            }
            return response;
        }
    }
}