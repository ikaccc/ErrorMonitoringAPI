using Newtonsoft.Json;

namespace ExceptionHandler.API.Models
{
    public class Trace
    {
        [JsonProperty("frames", Required = Required.Always)]
        public Frame[] Frames { get; set; }

        [JsonProperty("exception", Required = Required.Always)]
        public Exception Exception { get; set; }

        [JsonProperty("stackTrace", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StackTrace { get; set; }
    }
}
