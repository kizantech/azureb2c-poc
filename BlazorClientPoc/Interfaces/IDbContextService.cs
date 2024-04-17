using BlazorAppPoc.Models;

namespace BlazorAppPoc.Interfaces
{
    public interface  IDbContextService
    {
        public Task<IEnumerable<Todo>> GetTodos(int id);
    }
}
