using Newtonsoft.Json;

namespace ExceptionHandler.API.Models
{
    public class ErrorHandlerResponse
    {
        [JsonProperty("err")]
        public int Error { get; set; }

        public ErrorHandlerResult Result { get; set; }
    }
}
