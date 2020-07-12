using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExceptionHandler.API.Models
{
    public class WebUser
    {
        [JsonProperty("user_id", Required = Required.Always)]
        public Guid UserId { get; set; }

        public string FullName { get; set; }
        public string  TimeZoneId { get; set; }
        public bool SendAlerts { get; set; }
        public string Email { get; set; }
        public string AdditionalEmail { get; set; }
    }
}
