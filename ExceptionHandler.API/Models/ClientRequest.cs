namespace ExceptionHandler.API.Models
{
    public class ClientRequest
    {
        public int err { get; set; } = 0;
        public ErrorHandlerResult Result { get; set; }
    }
}
