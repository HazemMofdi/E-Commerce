﻿using Domain.Entities;
using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.Contracts;
using Shared.Common;
using Shared.DTOs.IdentityModule;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Implementations
{
    public class AuthenticationService(UserManager<ApplicationUser> userManager, IOptions<JwtOptions> options) : IAuthenticationService
    {
        public async Task<UserResultDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user is null) throw new UnauthorizedException();
            var result = await userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!result) throw new UnauthorizedException();
            return new UserResultDTO(user.Email, await CreateTokenAsync(user), user.DisplayName);
        }

        public async Task<UserResultDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName,
            };

            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e=> e.Description).ToList();
                throw new ValidationException(errors);
            }
            return new UserResultDTO(user.Email, await CreateTokenAsync(user), user.DisplayName);
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var jwtOptions = options.Value;
            // claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            // key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            // credintials
            var singInCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // token creation
            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                expires: DateTime.Now.AddDays(jwtOptions.ExpirationInDays),
                signingCredentials: singInCreds,
                claims: claims
                );
            // writeToken [Object Member Method]
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
