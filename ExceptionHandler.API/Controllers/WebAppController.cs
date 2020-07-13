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
        private readonly IPasswordHasher _passwordHasher;

        public WebAppController(ISecurity sequrity, IRequestHandler handleRequest, IPasswordHasher passwordHasher)
        {
            _sequrity = sequrity;
            _requestHandler = handleRequest;
            _passwordHasher = passwordHasher;
        }


        [HttpGet]
        public async Task<IActionResult> GetUserData([FromQuery] WebUser user)
        {
            var userData = await _requestHandler.GetWebUser(user.UserId);
            var test = "";
            try
            {
                test = _passwordHasher.Hash("IvanCekov1!");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            var tt = _passwordHasher.Check(
                test, "IvanCekov1!");
            return Ok(userData);
        }

        [HttpPost]
        public  async  Task<IActionResult> CreateUser([FromBody] )
    }
}
