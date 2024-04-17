using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using BlazorAppPoc.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace BlazorAppPoc.Services
{
    public class UserService
    {
        private ClaimsPrincipal currentUser = new(new ClaimsIdentity());
        
        private readonly AuthenticationStateProvider _stateProvider;
        public UserService(AuthenticationStateProvider stateProvider) 
        {            
            _stateProvider = stateProvider;
        }

        public ClaimsPrincipal GetUser()
        {
            return currentUser;
        }

        internal void SetUser(ClaimsPrincipal user)
        {
            if (currentUser != user)
                currentUser = user;
        }
        
        [Obsolete]
        public async Task<User> SetUser(User currentUser) 
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

    internal sealed class UserCircuitHandler(
        AuthenticationStateProvider authenticationStateProvider,
        UserService userService)
        : CircuitHandler, IDisposable
    {
        public override Task OnCircuitOpenedAsync(Circuit circuit,
            CancellationToken cancellationToken)
        {
            authenticationStateProvider.AuthenticationStateChanged += AuthenticationChanged;
            return base.OnCircuitOpenedAsync(circuit, cancellationToken);
        }

        private void AuthenticationChanged(Task<AuthenticationState> task)
        {
            _ = UpdateAuthentication(task);

            async Task UpdateAuthentication(Task<AuthenticationState> task)
            {
                try
                {
                    var state = await task;
                    userService.SetUser(state.User);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public override async Task OnConnectionUpAsync(Circuit circuit,
            CancellationToken cancellationToken)
        {
            var state = await authenticationStateProvider.GetAuthenticationStateAsync();
            userService.SetUser(state.User);
        }

        public void Dispose()
        {
            authenticationStateProvider.AuthenticationStateChanged -=
                AuthenticationChanged;
        }
    }
}
