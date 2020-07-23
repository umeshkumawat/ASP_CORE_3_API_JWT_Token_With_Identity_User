using IdentityWithJWT.ActionFilters;
using IdentityWithJWT.Contracts;
using IdentityWithJWT.DTO;
using IdentityWithJWT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityWithJWT.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationManager = authenticationManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistration userDTO)
        {
            var newUser = new User 
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                PhoneNumber = userDTO.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, userDTO.Password);

            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.TryAddModelError(error.Code, error.Description);
                return BadRequest(ModelState);
            }

            //Check if roles exists in Db
            var roleResult = new List<string>();
            foreach(var role in userDTO.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    roleResult.Add($"Role \"{role}\" does not exist.");
            }

            if (roleResult.Count() > 0)
                return BadRequest(roleResult);

            await _userManager.AddToRolesAsync(newUser, userDTO.Roles);

            return Ok();
        }

        [HttpPost, Route("token")]
        [ModelValidationActionFilter]
        public async Task<IActionResult> GetToken([FromBody] UserForAuthentication userForAuth)
        {
            if(!await _authenticationManager.ValidateUserAsync(userForAuth))
            {
                return Unauthorized();
            }

            var token = await _authenticationManager.CreateTokenAsync();

            return Ok(new { Token = token});
        }
    }
}
