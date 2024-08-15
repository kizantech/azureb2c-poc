using System.Security.Claims;
using AzureB2C.AuthApi.Utils;
using Microsoft.AspNetCore.Authentication;

namespace AzureB2C.AuthApi
{
    public class CustomClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = (ClaimsIdentity)principal.Identity;
            var roleClaim = identity.FindFirst($"extension_{Constants.UserAttributes.DelegatedUserManagementRole}");

            if (roleClaim != null)
            {
                identity.AddClaim(new Claim(identity.RoleClaimType, roleClaim.Value));
            }
            return Task.FromResult(principal);
        }
    }
}
