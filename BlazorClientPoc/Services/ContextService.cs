using BlazorAppPoc.Contexts;
using BlazorAppPoc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using BlazorAppPoc.Interfaces;


namespace BlazorAppPoc.Services
{
    public class ContextService
    {
        private readonly PocDbContext _dbContext;
        
        public ContextService( PocDbContext dbContext) 
        {
            _dbContext = dbContext;
            dbContext.Database.EnsureCreated();
        }

        public async Task<List<Todo>> GetTodos()
        {
            return await _dbContext.Todos.ToListAsync();
        }

        public async Task<Todo?> GetTodoById(int id) 
        {            
            return await _dbContext.Todos.FirstOrDefaultAsync(t => t.TenantId == id.ToString());
        }
    }
}
