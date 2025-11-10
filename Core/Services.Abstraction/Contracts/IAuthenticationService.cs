using Shared.DTOs.IdentityModule;
using Shared.DTOs.OrderModule;
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
        Task<UserResultDTO> GetCurrentUserAsync(string userEmail);
        Task<bool> CheckEmailExistsAsync(string userEmail);
        Task<ShippingAddressDTO> GetUserAddressAsync(string userEmail);
        Task<ShippingAddressDTO> UpdateUserAddressAsync(string userEmail, ShippingAddressDTO addressDTO);



    }
}
