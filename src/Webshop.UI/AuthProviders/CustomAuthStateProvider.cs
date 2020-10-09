using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Webshop.UI.Models;

namespace Webshop.UI.AuthProviders
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomAuthStateProvider> _logger;

        public CustomAuthStateProvider(HttpClient client,
            ILogger<CustomAuthStateProvider> logger)
        {
            _httpClient = client;
            _logger = logger;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return new AuthenticationState(await getUser());
        }

        private async Task<ClaimsPrincipal> getUser()
        {
            ApplicationUserModel user = null;
            try
            {
                _logger.LogInformation("Fetching user details from web api.");
                user = await _httpClient.GetFromJsonAsync<ApplicationUserModel>("Account");
            }
            catch (Exception exc)
            {
                _logger.LogWarning(exc, "Fetching user failed.");
            }

            if (user == null || !user.IsAuthenticated)
            {
                return null;
            }

            var identity = new ClaimsIdentity(
                nameof(CustomAuthStateProvider),
                user.NameClaimType,
                user.RoleClaimType);

            if (user.Claims != null)
            {
                foreach (var claim in user.Claims)
                {
                    identity.AddClaim(new Claim(claim.Type, claim.Value));
                }
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            return new ClaimsPrincipal(identity);
        }
    }
}
