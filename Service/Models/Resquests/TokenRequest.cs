using Newtonsoft.Json;

namespace Service.Models.Resquests
{
   public class TokenRequest
    {
        [JsonProperty("userName")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}