using System.Security.Claims;
using BlazorAppPoc.Services;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;

namespace BlazorAppPoc.Multitenant;

public class BlazorUserStrategy(UserService userService, IConfiguration configuration) : IMultiTenantStrategy
{
    public async Task<string?> GetIdentifierAsync(object context)
    {
        var user = userService.GetUser();
        var claim = user.FindFirst(configuration["ClaimSettings:TenantIdClaimType"]);
        return claim?.Value ?? null;
    }
}