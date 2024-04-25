using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace AzureB2C.Blazor.Services
{
    public class UserService
    {
        private ClaimsPrincipal? _currentUser;
        
        public ClaimsPrincipal? GetUser()
        {
            return _currentUser;
        }

        internal void SetUser(ClaimsPrincipal user)
        {
            _currentUser = user;
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
            return;

            async Task UpdateAuthentication(Task<AuthenticationState> authTask)
            {
                try
                {
                    var state = await authTask;
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
