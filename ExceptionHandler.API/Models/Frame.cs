using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExceptionHandler.API.Models
{
    public class Frame
    {
        [JsonProperty("filename", Required = Required.Always)]
        public string FileName { get; private set; }

        [JsonProperty("lineno", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? LineNo { get; set; }

        [JsonProperty("colno", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ColNo { get; set; }

        [JsonProperty("method", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Method { get; set; }

        [JsonProperty("code", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("context", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CodeContext Context { get; set; }

        [JsonProperty("args", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] Args { get; set; }

        [JsonProperty("kwargs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, object> Kwargs { get; set; }
    }
}
