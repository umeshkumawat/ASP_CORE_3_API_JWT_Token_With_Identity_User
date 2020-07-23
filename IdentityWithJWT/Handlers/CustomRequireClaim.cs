using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityWithJWT.Handlers
{
    public class CustomRequireClaim : IAuthorizationRequirement
    {
        public CustomRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }

    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequireClaim requirement)
        {
            var hasClaim = context.User.Claims.Any(c => c.Type.Equals(requirement.ClaimType));

            if (hasClaim)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
