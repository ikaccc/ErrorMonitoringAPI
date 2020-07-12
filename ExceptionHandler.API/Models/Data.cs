using ExceptionHandler.API.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ExceptionHandler.API.Models
{
    public class Data
    {
        [JsonProperty("dataId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid Id { get; set; }

        [JsonProperty("environment", Required = Required.Always)]
        public string Environment { get; private set; }

        [JsonProperty("body", Required = Required.Always)]
        public Body Body { get; private set; }

        [JsonProperty("level", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ErrorLevel? Level { get; set; }

        [JsonProperty("timestamp", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? Timestamp { get; set; }

        [JsonProperty("code_version", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CodeVersion { get; set; }

        [JsonProperty("platform", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Platform { get; set; }

        [JsonProperty("language", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Language { get; set; }

        [JsonProperty("framework", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Framework { get; set; }

        [JsonProperty("context", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Context { get; set; }

        [JsonProperty("person", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Person Person { get; set; }

        [JsonProperty("server", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Server Server { get; set; }

        [JsonProperty("custom", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, object> Custom { get; set; }

        [JsonProperty("fingerprint", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Fingerprint { get; set; }

        [JsonProperty("title", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("uuid", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("notifier", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, string> Notifier { get; private set; }

        [JsonProperty("isMailSend", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsMailSend { get; set; }

        [JsonProperty("sysDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime sysDate { get; set; }
    }
}
