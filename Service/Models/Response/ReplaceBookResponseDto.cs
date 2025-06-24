using Newtonsoft.Json;
using Service.Models.DTOs;


namespace Service.Models.Response
{
    public class ReplaceBookResponseDto
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("books")]
        public ICollection<BookDto> Books{ get; set; }
    }
}