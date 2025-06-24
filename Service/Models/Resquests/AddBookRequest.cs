using Newtonsoft.Json;

using Service.Models.DTOs;

namespace Service.Models.Resquests
{
    public class AddBookRequest
    {
        public AddBookRequest(string userId, string isbn)
        {
            this.userId = userId;
            collectionOfIsbns = new IsbnDto[]{new IsbnDto(isbn)};
        }
        public AddBookRequest(string userId, ICollection<string> isbns)
        {
            this.userId = userId;
            collectionOfIsbns = isbns.Select(isbn=>new IsbnDto(isbn)).ToArray();
        }
        [JsonProperty("userId")]
        public string userId { get; set; }
        [JsonProperty("collectionOfIsbns")]
        public IsbnDto[] collectionOfIsbns { get; set; }
    }
}