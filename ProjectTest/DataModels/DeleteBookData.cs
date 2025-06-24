using Newtonsoft.Json;

namespace ProjectTest.DataModels
{
    public class DeleteBookData
    {
        [JsonProperty("bookKey")]
        public string BookKey { get; set; }
        [JsonProperty("ownerAccKey")]
        public string OwnerAccountKey { get; set; }
        [JsonProperty("loginAccKey")]
        public string LoginAccountKey { get; set; }
    }
}