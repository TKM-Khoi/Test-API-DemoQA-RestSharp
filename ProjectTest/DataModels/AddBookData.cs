using Newtonsoft.Json;

namespace ProjectTest.DataModels
{
    /// <summary>
    /// The book will be added in the testing process or in the prep phase for the test
    /// </summary>
    public class AddBookData
    {
        public AddBookData(string bookKey, string ownerAccountKey, string loginAccountKey)
        {
            BookKey = bookKey;
            OwnerAccountKey = ownerAccountKey;
            LoginAccountKey = loginAccountKey;
        }

        [JsonProperty("bookKey")]
        public string BookKey { get; set; }
        [JsonProperty("ownerAccKey")]
        public string OwnerAccountKey { get; set; }
        [JsonProperty("loginAccKey")]
        public string LoginAccountKey { get; set; }
    }
}