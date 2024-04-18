using BlazorAppPoc.Models;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppPoc.Contexts
{
    public class PocDbContext : MultiTenantDbContext
    {
        public PocDbContext(ITenantInfo tenantInfo) : base(tenantInfo) { }
        public PocDbContext(DbContextOptions options, ITenantInfo tenantInfo) : base(tenantInfo, options) { }
        
        public DbSet<ToDo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            #if DEBUG
            // modelBuilder.Entity<ToDo>().HasData(                
            //     new ToDo { Title = "Work", Description = "Go To Work", CompletedDateTimeStamp = null, IsCompleted = false },
            //     new ToDo { Title = "Standup Meeting", Description = "Attend Morning Standup", CompletedDateTimeStamp = null, IsCompleted = false }, 
            //     new ToDo { Title = "Workout", Description = "Go To The Gym", CompletedDateTimeStamp = null, IsCompleted = false },
            //     new ToDo { Title = "Watch Basketball", Description = "Watch March Madness!", CompletedDateTimeStamp = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(5)), IsCompleted = true },
            //     new ToDo { Title = "Quiet Time", Description = "Read a book, or put together a puzzle", CompletedDateTimeStamp = DateTimeOffset.Now, IsCompleted = true }               
            //     );
            #endif
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(TenantInfo.ConnectionString);
        }
    }
}
