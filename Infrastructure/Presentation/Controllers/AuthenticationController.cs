using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.DTOs.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDTO>> LoginAsync(LoginDTO loginDTO)
        {
            return Ok(await serviceManager.AuthenticationService.LoginAsync(loginDTO));
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            return Ok(await serviceManager.AuthenticationService.RegisterAsync(registerDTO));
        }
    }
}
