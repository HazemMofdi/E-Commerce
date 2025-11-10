using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.IdentityModule
{
    public record LoginDTO
    {
        //             We User init To Make the Property Immutable
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
