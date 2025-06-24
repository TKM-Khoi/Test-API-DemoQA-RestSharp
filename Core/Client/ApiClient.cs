using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using RestSharp.Serializers.NewtonsoftJson;

namespace Core.Client
{
    public class ApiClient
    {
        private readonly RestClient _client;
        public RestRequest Request;
        private string _baseUrl = string.Empty;
        public ApiClient(RestClient client)
        {
            _client = client;
            Request = new RestRequest();
        }
        public ApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            var opts = new RestClientOptions(baseUrl);
            _client = new RestClient(opts, configureSerialization: cs=>cs.UseNewtonsoftJson());
            Request = new RestRequest();
        }
        private ApiClient(RestClientOptions opts)
        {
            _client = new RestClient(opts, configureSerialization: cs=>cs.UseNewtonsoftJson());
            Request = new RestRequest();
        }
        public ApiClient SetBasicAuthentication(string username, string password)
        {
            var options = new RestClientOptions(_baseUrl);
            options.Authenticator = new HttpBasicAuthenticator(username, password);
            return new ApiClient(options);
        }
        public ApiClient SetRequestTokenAuthentication(string comsumerKey, string consumerSecret)
        {
            var options = new RestClientOptions(_baseUrl);
            options.Authenticator = OAuth1Authenticator.ForRequestToken(comsumerKey, consumerSecret);
            return new ApiClient(options);
        }
        public ApiClient SetAccessTokenAuthentication(string comsumerKey, string consumerSecret, string oauthToken, string oauthSecret)
        {
            var options = new RestClientOptions(_baseUrl);
            options.Authenticator = OAuth1Authenticator.ForAccessToken(comsumerKey, consumerSecret, oauthToken, oauthSecret);
            return new ApiClient(options);
        }
        public ApiClient SetRequestHeaderAuthentication(string token, string authType = "Bearer")
        {
            var options = new RestClientOptions(_baseUrl);
            options.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, authType);
            return new ApiClient(options);
        }
        public ApiClient SetJwtAuthentication(string jwt)
        {
            var options = new RestClientOptions(_baseUrl);
            options.Authenticator = new JwtAuthenticator(jwt);
            return new ApiClient(options);
        }
        public ApiClient ClearAuthenticator()
        {
            var opts = new RestClientOptions(_baseUrl);
            return new ApiClient(opts);
        }


        public ApiClient AddDefaultHeaders(Dictionary<string, string> headers)
        {
            _client.AddDefaultHeaders(headers);
            return this;
        }
        public ApiClient AddHeader(string name, string value)
        {
            Request.AddHeader(name, value);
            return this;
        }
        public ApiClient CreateRequest(string source = "")
        {
            Request = new RestRequest(source);
            return this;
        }

        public ApiClient AddAuthorizationHeader(string value)
        {
            return AddHeader("Authorization", value);
        }
        public ApiClient AddContentType(string contentType = null)
        {
            return AddHeader("Content-Type", contentType ?? ContentType.Json);
        }
        public ApiClient AddParameter(string name, string value)
        {
            Request.AddParameter(name, value);
            return this;
        }
        public ApiClient AddBody(object body, string contentType = null)
        {
            Request.AddBody(body, contentType ?? ContentType.Json);
            return this;
        }
        public ApiClient AddStringBody(string body, string contentType = null)
        {
            Request.AddStringBody(body, contentType ?? ContentType.Json);
            return this;
        }
        public ApiClient AddJsonBody<T>(T body, string contentType = null) where T : class
        {
            Request.AddJsonBody(body );
            return this;
        }
        public ApiClient AddXmlBody<T>(T body, string contentType = null) where T : class
        {
            Request.AddXmlBody<T>(body, contentType ?? ContentType.Json);
            return this;
        }


        public RestResponse ExecuteGet()
        {
            return _client.ExecuteGet(Request);
        }
        public async Task<RestResponse> ExecuteGetAsync()
        {
            return await _client.ExecuteGetAsync(Request);
        }
        public RestResponse<T> ExecuteGet<T>()
        {
            return _client.ExecuteGet<T>(Request);
        }
        public async Task<RestResponse<T>> ExecuteGetAsync<T>()
        {
            return await _client.ExecuteGetAsync<T>(Request);
        }

        public RestResponse ExecutePost()
        {
            return _client.ExecutePost(Request);
        }
        public RestResponse<T> ExecutePost<T>()
        {
            return _client.ExecutePost<T>(Request);
        }
        public async Task<RestResponse> ExecutePostAsync()
        {
            return await _client.ExecutePostAsync(Request);
        }
        public async Task<RestResponse<T>> ExecutePostAsync<T>()
        {
            return await _client.ExecutePostAsync<T>(Request);
        }
        public RestResponse ExecutePut()
        {
            return _client.ExecutePut(Request);
        }
        public async Task<RestResponse> ExecutePutAsync()
        {
            return await _client.ExecutePutAsync(Request);
        }
          public RestResponse<T> ExecutePut<T>()
        {
            return _client.ExecutePut<T>(Request);
        }
        public async Task<RestResponse<T>> ExecutePutAsync<T>()
        {
            return await _client.ExecutePutAsync<T>(Request);
        }
        public RestResponse ExecutePatch()
        {
            return _client.ExecutePatch(Request);
        }
        public async Task<RestResponse> ExecutePatchAsync()
        {
            return await _client.ExecutePatchAsync(Request);
        }
        public RestResponse ExecuteDelete()
        {
            return _client.ExecuteDelete(Request);
        }
        public async Task<RestResponse> ExecuteDeleteAsync()
        {
            return await _client.ExecuteDeleteAsync(Request);
        }
    }
}