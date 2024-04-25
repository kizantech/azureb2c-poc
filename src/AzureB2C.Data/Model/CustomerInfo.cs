using Finbuckle.MultiTenant.Abstractions;

namespace AzureB2C.Data.Model;

public class CustomerInfo : ITenantInfo
{
    public string? Id { get; set; }
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public Guid CustomerId { get; set; }
    public string? ConnectionString { get; set; }
}