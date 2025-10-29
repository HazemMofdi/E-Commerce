using Shared.DTOs.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IAuthenticationService
    {
        Task<UserResultDTO> LoginAsync(LoginDTO loginDTO);
        Task<UserResultDTO> RegisterAsync(RegisterDTO registerDTO);
    }
}
