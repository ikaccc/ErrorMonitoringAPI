using Newtonsoft.Json;

namespace ExceptionHandler.API.Models
{
    public class Payload
    {
        [JsonProperty("access_token", Required = Required.Always)]
        public string AccessToken { get; set; }

        [JsonProperty("data", Required = Required.Always)]
        public Data Data { get; set; }
    }
}
