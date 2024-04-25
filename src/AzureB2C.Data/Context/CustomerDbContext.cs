using AzureB2C.Data.Model;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AzureB2C.Data.Context
{
    public class CustomerDbContext : MultiTenantIdentityDbContext
    {
        private CustomerInfo CustomerInfo { get; set; }

        public CustomerDbContext(CustomerInfo customerInfo) : base(customerInfo)
        {
            CustomerInfo = customerInfo;
        }

        public CustomerDbContext(CustomerInfo customerInfo, DbContextOptions<CustomerDbContext> options) : base(customerInfo, options)
        {
            CustomerInfo = customerInfo;
        }
        
        public DbSet<ToDo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (CustomerInfo?.ConnectionString != null)
                optionsBuilder.UseSqlServer(CustomerInfo.ConnectionString);
        }
    }
}
