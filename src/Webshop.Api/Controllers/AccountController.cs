using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<ApplicationUserModel> Get()
        {
            var signInResult = await _signInManager.PasswordSignInAsync("TestUser", "Pwd12345.", true, false);


            return await Task.FromResult(new ApplicationUserModel()
            {
                UserName = "Test",
                Token = "123",
                IsAuthenticated = true,
                NameClaimType = "Admin",
                RoleClaimType = "1"
            });
        }

        [HttpPost]
        public async Task<ApplicationUserModel> Login(LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);

            return await Task.FromResult(new ApplicationUserModel()
            {
                UserName = "Test",
                Token = "123",
                IsAuthenticated = result.Succeeded,
                NameClaimType = "Admin",
                RoleClaimType = "1"
            });
        }
    }
}
