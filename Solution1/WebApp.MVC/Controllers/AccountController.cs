using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.MVC.Services;

namespace WebApp.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenService _tokenService;

        public AccountController(ITokenService tokenService, ILogger<HomeController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        public ActionResult Login()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = "Home/Index" },"oidc");
        }

        public ActionResult Logout()
        {
            return SignOut(new AuthenticationProperties() { RedirectUri = "Home/Index" },"oidc", "cookie");
        }

        [Authorize]
        public async Task<IActionResult> Weather()
        {
            using (var client = new HttpClient())
            {
                var tokenResponse = await _tokenService.GetToken("read");

                client
                  .SetBearerToken(tokenResponse.AccessToken);

                var result = client
                  .GetAsync("https://localhost:44363/weatherforecast")
                  .Result;

                if (result.IsSuccessStatusCode)
                {
                    var model = result.Content.ReadAsStringAsync().Result;

                    return View(model);
                }
                else
                {
                    throw new Exception("Unable to get content");
                }

            }
        }
    }
}
