using Newtonsoft.Json;

namespace ProjectTest.DataModels
{
    public class ReplaceBookData
    {
        [JsonProperty("oldBookKey")]
        public string OldBookKey { get; set; }
        [JsonProperty("newBookKey")]
        public string NewBookKey { get; set; }
        [JsonProperty("ownerAccKey")]
        public string OwnerAccountKey { get; set; }
        [JsonProperty("loginAccKey")]
        public string LoginAccountKey { get; set; }
    }
}