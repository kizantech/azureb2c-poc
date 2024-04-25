using AzureB2C.Blazor.Services;
using Finbuckle.MultiTenant.Abstractions;

namespace AzureB2C.Blazor.Multitenant;

public class BlazorUserStrategy(UserService userService, IConfiguration configuration) : IMultiTenantStrategy
{
    public async Task<string?> GetIdentifierAsync(object context)
    {
        var user = userService.GetUser();
        if (user == null) return null;
        var claim = user.FindFirst(configuration["ClaimSettings:TenantIdClaimType"]);
        return claim?.Value ?? null;
    }
}