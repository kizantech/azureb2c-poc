using Finbuckle.MultiTenant.Abstractions;

namespace AzureAdB2BApi.Models;

public class TenantInfo : ITenantInfo
{
    public string? Id { get; set; }
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public Guid CustomerId { get; set; }
    public string? ConnectionString { get; set; }
}