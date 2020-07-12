using System;

namespace ExceptionHandler.API.Models
{
    public class RegistredApplications
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public string AccessToken { get; set; }
        public bool IsTrial { get; set; }
        public int TrialDuration { get; set; }
        public Guid ConcurrencyStamp { get; set; } = Guid.Empty;
        public string ApplicationName { get; set; }
        public string DbServer { get; set; }
        public string DbName { get; set; }
        public string DbUserName { get; set; }
        public string DbPassword { get; set; }
        public DateTime DateRegistred { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
