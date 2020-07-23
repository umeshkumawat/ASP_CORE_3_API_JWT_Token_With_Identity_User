using IdentityWithJWT.Contracts;
using IdentityWithJWT.DTO;
using IdentityWithJWT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityWithJWT
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;

        private User _user;

        public AuthenticationManager(UserManager<User> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> CreateTokenAsync()
        {
            var roles = await _userManager.GetRolesAsync(_user);

            // Generate Claims
            var userClaims = new List<Claim>();

            userClaims.Add(new Claim(ClaimTypes.Name, _user.UserName));

            foreach (var role in roles)
                userClaims.Add(new Claim(ClaimTypes.Role, role));

            // Add one Fake claim
            userClaims.Add(new Claim("FakeClaim", "Fake Value"));

            // Create Signing Credentials
            var cred = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var scred = new SigningCredentials(cred, SecurityAlgorithms.HmacSha256);


            var jwtToken = new JwtSecurityToken
                (
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: userClaims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: scred
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);

        }

        public async Task<bool> ValidateUserAsync(UserForAuthentication userForAuth)
        {
            _user = await _userManager.FindByNameAsync(userForAuth.UserName);

            return (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));
        }
    }
}
