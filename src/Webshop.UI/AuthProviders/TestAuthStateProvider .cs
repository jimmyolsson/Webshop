using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Webshop.UI.AuthProviders
{
    public class TestAuthStateProvider : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(1500);

            var anonymous = new ClaimsIdentity();
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
        }
    }
}
