using ExceptionHandler.DataAccess;

namespace ExceptionHandler.API.Models
{
    public class ApplicationDbConfig
    {
        [ResultParameter("DbServer")]
        public string DbServerName { get; set; }
        [ResultParameter("DbName")]
        public string DbName { get; set; }
        [ResultParameter("DbUserName")]
        public string DbUserName { get; set; }
        [ResultParameter("DbPassword")]
        public string DbPassword { get; set; }
    }
}
