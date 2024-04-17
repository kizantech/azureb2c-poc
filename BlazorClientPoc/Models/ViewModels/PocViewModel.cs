using Blazing.Mvvm.ComponentModel;
using BlazorAppPoc.Services;
using Microsoft.IdentityModel.Tokens;

namespace BlazorAppPoc.Models.ViewModels
{
    public class PocViewModel: ViewModelBase
    {
        private Todo _todo;
        private readonly ContextService _contextService;
        private readonly UserService _userService;
        
        public PocViewModel(ContextService contextService, UserService userService) 
        {
            _contextService = contextService;
            _userService = userService;
            _todo = new Todo();
        }
        
        public User User { get; private set; } = new User();
        
        public async Task SetUser()
        {
           User = await _userService.SetUser(User);
        }
    }
}
