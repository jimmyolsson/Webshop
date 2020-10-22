using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Webshop.Api.Entities;
using Webshop.UI.Models;

namespace Webshop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<WeatherForecastController> _logger;

        public AccountController(SignInManager<ApplicationUser> signInManager,
                                    UserManager<ApplicationUser> userManager,
                                 ILogger<WeatherForecastController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public ApplicationUserModel Get()
        {
            if (!User.Identity.IsAuthenticated)
                return new ApplicationUserModel
                {
                    IsAuthenticated = false
                };

            return createUser(User);
        }

        private ApplicationUserModel createUser(ClaimsPrincipal claimsPrincipal)
        {
            if (!claimsPrincipal.Identity.IsAuthenticated)
            {
                return new ApplicationUserModel
                {
                    IsAuthenticated = false
                };
            }

            var user = new ApplicationUserModel
            {
                IsAuthenticated = true
            };

            if (claimsPrincipal.Identity is ClaimsIdentity claimsIdentity)
            {
                user.NameClaimType = claimsIdentity.NameClaimType;
                user.RoleClaimType = claimsIdentity.RoleClaimType;
            }
            else
            {
                user.NameClaimType = "name";
                user.RoleClaimType = "role";
            }

            if (claimsPrincipal.Claims.Any())
            {
                var claims = new List<ApplicationClaims>();
                var nameClaims = claimsPrincipal.FindAll(user.NameClaimType);
                foreach (var claim in nameClaims)
                {
                    claims.Add(new ApplicationClaims(user.NameClaimType, claim.Value));
                }

                foreach (var claim in claimsPrincipal.Claims.Except(nameClaims))
                {
                    claims.Add(new ApplicationClaims(claim.Type, claim.Value));
                }

                user.Claims = claims;
            }

            // Should we have tokens? 
            //var token = await _tokenClaimsService.GetTokenAsync(claimsPrincipal.Identity.Name);
            //user.Token = token;

            return user;
        }

        [HttpPost]
        public async Task<LoginResult> Login(LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);

            return await Task.FromResult(new LoginResult(request.UserName, result.Succeeded));
        }
    }
}
