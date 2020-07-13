using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExceptionHandler.API.Helpers;
using ExceptionHandler.API.Intefaces;
using ExceptionHandler.API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ExceptionHandler.API.Services
{
    public class UserService
    {
        private readonly IDataManager _dataManager;
        private readonly AppSettings _appSettings;

        public UserService(IDataManager dataManager,
            IOptions<AppSettings> appSettings)
        {
            _dataManager = dataManager;
            _appSettings = appSettings.Value;
        }

        //public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        //{
        //    AuthenticateResponse user = await _dataManager.GetUserByUserAndPass(model.Username, model.Password);

        //    if (user == null) return null;

        //    var token = generateJwtToken(user);

        //    return new AuthenticateResponse(user, token);
        //}

        //private string generateJwtToken(User user)
        //{
        //    // generate token that is valid for 7 days
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name, user.Id.ToString())
        //        }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
    }
}
