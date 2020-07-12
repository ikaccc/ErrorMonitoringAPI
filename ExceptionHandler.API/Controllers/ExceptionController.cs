using ExceptionHandler.API.Intefaces;
using ExceptionHandler.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExceptionHandler.API.Controllers
{
    [Route("api/exception")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {
        private readonly ISecurity _sequrity;
        private readonly IRequestHandler _requestHandler;

        public ExceptionController(ISecurity sequrity, IRequestHandler handleRequest)
        {
            _sequrity = sequrity;
            _requestHandler = handleRequest;
        }

        [HttpPost]
        public async Task<IActionResult> CreateException([FromBody] Payload data)
        {

            try
            {
                var isAuth = await _sequrity.CheckTokenAndUser(data.AccessToken);

                if (!isAuth)
                {
                    return Forbid("Access Denied");
                }
                var processData = await _requestHandler.ProcessData(data);

                return Ok(new ErrorHandlerResponse
                {
                    Error = 0,
                    Result = new ErrorHandlerResult
                    {
                        Uuid = data.Data.Uuid
                    }
                });
            }
            catch (System.Exception e)
            {
                return Ok(new ErrorHandlerResponse
                {
                    Error = 0,
                    Result = new ErrorHandlerResult
                    {
                        Uuid = data.Data.Uuid
                    }
                });
            }
        }
    }
}
