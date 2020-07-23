using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityWithJWT.Controllers
{
    [Route("api/secret")]
    [ApiController]
    public class SecretController : ControllerBase
    {
        [HttpGet("SecretWithPolicy"), Authorize(Policy = "Fake")]
        public IActionResult Secret()
        {
            return Ok("No Secrets");
        }

        [HttpGet("SecretWithRole"), Authorize(Roles = "Manager")]
        public IActionResult Secret1()
        {
            return Ok("No Secrets");
        }
    }
}
