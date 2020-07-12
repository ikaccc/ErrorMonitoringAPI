using ExceptionHandler.API.Intefaces;
using ExceptionHandler.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExceptionHandler.API.Controllers
{
    [Route("api/exception/data")]
    [ApiController]
    public class WebAppController : ControllerBase
    {
        private readonly ISecurity _sequrity;
        private readonly IRequestHandler _requestHandler;

        public WebAppController(ISecurity sequrity, IRequestHandler handleRequest)
        {
            _sequrity = sequrity;
            _requestHandler = handleRequest;
        }


        [HttpGet]
        public async Task<IActionResult> GetUserData([FromQuery] WebUser user)
        {
            var userData = await _requestHandler.GetWebUser(user.UserId);

            return Ok(userData);
        }
    }
}
