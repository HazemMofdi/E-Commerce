using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.DTOs.IdentityModule;
using Shared.DTOs.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string userEmail)
        {
            return Ok(await serviceManager.AuthenticationService.CheckEmailExistsAsync(userEmail));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDTO>> GetCurrencUserAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await serviceManager.AuthenticationService.GetCurrentUserAsync(email);
            return Ok(user);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<ShippingAddressDTO>> GetUserAddressAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await serviceManager.AuthenticationService.GetUserAddressAsync(email);
            return Ok(address);
        }


        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<ShippingAddressDTO>> UpdateUserAddressAsync(ShippingAddressDTO addressDTO)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await serviceManager.AuthenticationService.UpdateUserAddressAsync(email, addressDTO);
            return Ok(address);
        }
    }
}
