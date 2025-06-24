using Newtonsoft.Json;

using Service.Models.DTOs;

namespace Service.Models.Response
{
    public class AddBookResponseDto
    {
        [JsonProperty("books")]
        public IsbnDto[] CollectionOfIsbns { get; set; }
    }
}