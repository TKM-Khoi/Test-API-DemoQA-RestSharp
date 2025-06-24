using Newtonsoft.Json;

namespace ProjectTest.DataModels
{
    public class GetAccountDetailData
    {
        [JsonProperty("accWithUserIdKey")]
        public string AccountWithUserIdKey { get; set; }
        [JsonProperty("loginAccKey")]
        public string LoginAccountKey { get; set; }
        [JsonProperty("bookKeys")]
        public ICollection<string> BookKeys { get; set; }
        
    }
}