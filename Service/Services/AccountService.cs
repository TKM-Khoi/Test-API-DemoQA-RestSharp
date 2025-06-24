using System.Net;

using Core.Client;

using RestSharp;

using Service.Const;
using Service.Models.Response;
using Service.Models.Resquests;

namespace Service.Services
{
    public class AccountService
    {
        private ApiClient _client;

        public AccountService(ApiClient client)
        {
            _client = client;
        }
        public async Task<RestResponse<UserDetailResponseDto>> GetDetailUserWithUnameAndPasswordAsync(string userId, string username, string password)
        {
            return await _client
                .SetBasicAuthentication(username, password)
                .CreateRequest(EndpointConst.GET_USER_DETAL_API(userId))
                .AddContentType(ContentType.Json)
                .ExecuteGetAsync<UserDetailResponseDto>();
        }
        public async Task<RestResponse<UserDetailResponseDto>> GetDetailUserWithTokenAsync(string userId, string token)
        {
            return await _client.CreateRequest(EndpointConst.GET_USER_DETAL_API(userId))
                .AddContentType(ContentType.Json)
                .AddAuthorizationHeader(token)
                .ExecuteGetAsync<UserDetailResponseDto>();
        }
        public async Task<RestResponse<TokenResponseDto>> GenerateTokenAsync(string username, string password)
        {
            TokenRequest tokenRequest = new()
            {
                Username = username,
                Password = password
            };
            return await _client.CreateRequest(EndpointConst.GEN_JWT_TOKEN_API)
                .AddHeader("accept", ContentType.Json)
                .AddContentType(ContentType.Json)
                .AddJsonBody(tokenRequest)
                .ExecutePostAsync<TokenResponseDto>();
        }

        /// <summary>
        /// GenerateToken API somtimes does not work consistently. I will call that api until it works or past 5 times
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="allowedAttempts"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<RestResponse<TokenResponseDto>> TryHardGenerateTokenAsync(string username, string password, int allowedAttempts = 5)
        {
            int count = 1;
            RestResponse<TokenResponseDto> response = null;
            while (count <= allowedAttempts)
            {
                response = await GenerateTokenAsync(username, password);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response;
                }
                else
                {
                    Thread.Sleep(4000);
                    count++;
                }
            }
            throw new Exception("Cant get token from API: " + response.Content);
        }
    }
}