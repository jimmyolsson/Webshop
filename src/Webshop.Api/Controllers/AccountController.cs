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
        private readonly ILogger<WeatherForecastController> _logger;

        public AccountController(SignInManager<ApplicationUser> signInManager,
                                 ILogger<WeatherForecastController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ApplicationUserModel> Get()
        {
            var signInResult = await _signInManager.PasswordSignInAsync("test", "Pwd12345.", true, false);


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
        public async Task<ApplicationUserModel> Login(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, true);


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
