using Newtonsoft.Json;

namespace Service.Models.DTOs
{
    public class IsbnDto
    {
        [JsonProperty("isbn")]
        public string isbn { get; set; }

        public IsbnDto(string isbn)
        {
            this.isbn = isbn;
        }
    }
}