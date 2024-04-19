using AzureAdB2BApi.Models;

namespace AzureAdB2BApi.Interfaces;

public interface ITenantService
{
    public Task<Tenant> GetTenant(User user);
}