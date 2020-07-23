using IdentityWithJWT.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityWithJWT.Models
{
    public class JwtSettings
    {
        private readonly IConfiguration _config;

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }

    }
}
