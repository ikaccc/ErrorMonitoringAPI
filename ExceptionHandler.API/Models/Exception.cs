using Newtonsoft.Json;

namespace ExceptionHandler.API.Models
{
    public class Exception
    {
        [JsonProperty("class", Required = Required.Always)]
        public string Class { get; private set; }

        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("stackTrace", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StackTrace { get; set; }
    }
}
