using Newtonsoft.Json;

namespace ExceptionHandler.API.Models
{
    public class CrashReport
    {
        [JsonProperty("raw", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Raw { get; set; }
    }
}
