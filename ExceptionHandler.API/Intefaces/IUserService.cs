﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExceptionHandler.API.Models;

namespace ExceptionHandler.API.Intefaces
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
    }
}
