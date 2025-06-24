using Core.Client;
using Core.ShareData;

using RestSharp;

using Service.Const;
using Service.Models.DTOs;
using Service.Models.Response;
using Service.Models.Resquests;

namespace Service.Services
{
    public class BookService
    {
        private readonly ApiClient _client;

        public BookService(ApiClient client)
        {
            _client = client;
        }
        public async Task<RestResponse<AddBookResponseDto>> AddBookWithTokenAsync(AddBookRequest data, string token)
        {
            return await _client.CreateRequest(EndpointConst.ADD_BOOK_API)
                .AddHeader("accept", ContentType.Json)
                .AddHeader("Content-Type", ContentType.Json)
                .AddAuthorizationHeader(token)
                .AddBody(data)
                .ExecutePostAsync<AddBookResponseDto>();
        }
        public async Task<RestResponse<AddBookResponseDto>> AddBookWithUnameAndPasswordAsync(AddBookRequest data, string username, string password)
        {
            var response = await _client
                .SetBasicAuthentication(username, password)
                .CreateRequest(EndpointConst.ADD_BOOK_API)
                .AddHeader("accept", ContentType.Json)
                .AddHeader("Content-Type", ContentType.Json)
                .AddBody(data)
                .ExecutePostAsync<AddBookResponseDto>();
            return response;
        }
        public async Task<RestResponse> DeleteBookWithTokenAsync(string userId, string isbn, string token)
        {
            return await _client
                .CreateRequest(EndpointConst.DELETE_BOOK_API)
                .AddHeader("accept", ContentType.Json)
                .AddHeader("Content-Type", ContentType.Json)
                .AddAuthorizationHeader(token)
                .AddBody(new
                {
                    userId = userId,
                    isbn = isbn
                })
                .ExecuteDeleteAsync();
        }
        
        public async Task<RestResponse<ReplaceBookResponseDto>> ReplaceBookUsingUnameAndPassAsync(string userId, string oldIsbn, string newIsbn, string username, string password)
        {
            return await _client
                .SetBasicAuthentication(username, password)
                .CreateRequest(EndpointConst.REPLACE_BOOK_API(oldIsbn))
                .AddHeader("accept", ContentType.Json)
                .AddHeader("Content-Type", ContentType.Json)
                .AddBody(new
                {
                    userId = userId,
                    isbn = newIsbn
                })
                .ExecutePutAsync<ReplaceBookResponseDto>();
        }
        public async Task<RestResponse> DeleteBookWithUnameAndPassAsync(string userId, string isbn, string username, string password)
        {
            return await _client
                .SetBasicAuthentication(username, password)
                .CreateRequest(EndpointConst.DELETE_BOOK_API)
                .AddHeader("accept", ContentType.Json)
                .AddHeader("Content-Type", ContentType.Json)
                .AddBody(new
                {
                    userId = userId,
                    isbn = isbn
                })
                .ExecuteDeleteAsync();
        }
        public RestResponse DeleteAllBooksWithUnameAndPass(string userId, string username, string password)
        {
            return _client
                .SetBasicAuthentication(username, password)
                .CreateRequest(EndpointConst.DELETE_ALL_BOOKS_API)
                .AddHeader("accept", ContentType.Json)
                .AddHeader("Content-Type", ContentType.Json)
                .AddParameter("UserId", userId)
                .ExecuteDelete();
        }
        public RestResponse DeleteAllBooksWithToken(string userId, string token)
        {
            return _client
                .CreateRequest(EndpointConst.DELETE_ALL_BOOKS_API)
                .AddHeader("accept", ContentType.Json)
                .AddHeader("Content-Type", ContentType.Json)
                .AddAuthorizationHeader(token)
                .AddParameter("UserId", userId)
                .ExecuteDelete();
        }

        public void StoreUserToDeleteBookLater(string userId, string username, string password, string featureName)
        {
            if (DataStorage.GetData(DataStorage.CREATED_BOOKS_USERS_KEY(featureName)) is null)
            {
                DataStorage.SetData(DataStorage.CREATED_BOOKS_USERS_KEY(featureName), new HashSet<AccountDto>());
            }
            ((HashSet<AccountDto>)DataStorage.GetData(DataStorage.CREATED_BOOKS_USERS_KEY(featureName)))
                .Add(new AccountDto { UserId = userId, Username = username, Password = password });
        }
        public void DeleteCreatedBooksFromStorage(string featureName)
        {
            if (DataStorage.GetData(DataStorage.CREATED_BOOKS_USERS_KEY(featureName)) is not null)
            {
                var users = (HashSet<AccountDto>)DataStorage.GetData(DataStorage.CREATED_BOOKS_USERS_KEY(featureName));
                foreach (var account in users)
                {
                    DeleteAllBooksWithUnameAndPass(account.UserId, account.Username, account.Password);
                    users.Remove(account);
                }
            }
        }
    }
}