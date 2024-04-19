using AzureAdB2BApi.Contexts;
using AzureAdB2BApi.Interfaces;
using AzureAdB2BApi.Models;

namespace AzureAdB2BApi.Services;

public class TenantService: ITenantService
{
    private readonly TenantDbContext _tenantDbContext;
    public async Task<Tenant> GetTenant( User user)
    {
        Tenant tenant = new Tenant();
        
        //why am I having to use an explicit cast?
        tenant = (Tenant)_tenantDbContext.Tenants.Where(x => x.ProviderName == user.identityprovider);
        return tenant;
    }
}