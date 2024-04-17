using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using BlazorAppPoc.Models;

namespace BlazorAppPoc.Services
{
    public class UserService
    {
        private readonly AuthenticationStateProvider _stateProvider;
        public UserService(AuthenticationStateProvider stateProvider) 
        {            
            _stateProvider = stateProvider;
        }
        
        public async Task<User> setUser(User currentUser) 
        {
            var authState = await _stateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            currentUser.FirstName = user.FindFirst(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
            currentUser.LastName= user.FindFirst(c => c.Type == ClaimTypes.Surname)?.Value;
            currentUser.Identityprovider = user.FindFirst(c => c.Type == "http://schemas.microsoft.com/identity/claims/identityprovider")?.Value;
            currentUser.Email = user.FindFirst(c => c.Type == "emails")?.Value;

            return currentUser;
        }

       

        
        
       

    }
}
