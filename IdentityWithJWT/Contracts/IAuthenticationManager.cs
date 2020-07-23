using IdentityWithJWT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityWithJWT.Contracts
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUserAsync(UserForAuthentication userForAuth);

        Task<string> CreateTokenAsync();
    }
}
