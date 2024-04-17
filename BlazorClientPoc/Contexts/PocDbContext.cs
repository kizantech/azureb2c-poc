using BlazorAppPoc.Models;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppPoc.Contexts
{
    public class PocDbContext : DbContext
    {
        private TenantInfo _tenantInfo;
       
        public PocDbContext(TenantInfo tenantInfo)
        {
            _tenantInfo = tenantInfo;
        }

        public PocDbContext(DbContextOptions options, TenantInfo tenantInfo) : base(options)
        {
            _tenantInfo = tenantInfo;
        }
        
        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Todo>().HasData(                
                new Todo { TenantId = "one", Title = "Work", Description = "Go To Work" },
                new Todo { TenantId = "two", Title = "Standup Meeting", Description = "Attend Morning Standup" }, 
                new Todo { TenantId = "three", Title = "Workout", Description = "Go To The Gym" },
                new Todo { TenantId = "four", Title = "Watch Basketball", Description = "Watch March Madness!" },
                new Todo { TenantId = "five", Title = "Quiet Time", Description = "Read a book, or put together a puzzle" }               
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("pocdb");
        }
    }
}
