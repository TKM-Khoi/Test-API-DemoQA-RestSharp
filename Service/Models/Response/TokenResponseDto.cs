using Newtonsoft.Json;

namespace Service.Models.Response
{
    public class TokenResponseDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("expires")]
        public string Expires { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}