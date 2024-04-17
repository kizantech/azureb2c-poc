using BlazorAppPoc.Services;
using Finbuckle.MultiTenant;

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