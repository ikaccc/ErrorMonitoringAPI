using Newtonsoft.Json;

namespace ExceptionHandler.API.Models
{
    public class Body
    {
        [JsonProperty("trace", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Trace Trace { get; private set; }

        [JsonProperty("trace_chain", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Trace[] TraceChain { get; private set; }

        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message Message { get; private set; }

        [JsonProperty("crash_report", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CrashReport CrashReport { get; private set; }
    }
}
