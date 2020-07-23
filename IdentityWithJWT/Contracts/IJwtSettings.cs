using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityWithJWT.Contracts
{
    public interface IJwtSettings
    {
        string Audience { get; set; }
        string Issuer { get; set; }
        string Secret { get; set; }
    }
}
