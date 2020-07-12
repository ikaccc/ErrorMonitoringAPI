using Newtonsoft.Json;

namespace ExceptionHandler.API.Models
{
    public class Person
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("username", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string UserName { get; set; }
        [JsonProperty("email", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("userAgent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string UserAgent { get; set; }
        [JsonProperty("ipAddress", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string IpAddress { get; set; }
    }
}
