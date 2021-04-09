using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.MVC.Models;
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
            return Challenge(new AuthenticationProperties() { RedirectUri = "Home/Index" }, "oidc");
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    Uri u = new Uri("https://localhost:44311/api/account/registeruser");

                    var json = JsonConvert.SerializeObject(new { registerViewModel.Email, registerViewModel.Password });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    //var content = new FormUrlEncodedContent(new[]
                    //                {
                    //                    new KeyValuePair<string, string>("email",registerViewModel.email),
                    //                    new KeyValuePair<string, string>("password", registerViewModel.password)
                    //                  });
                    //HTTP POST
                    var postTask = await client.PostAsync(u, content);
                    //postTask.Wait();
                    string result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("RegisterConfirmation");
                    }
                   
                    ModelState.AddModelError(string.Empty, result);
                }
            }          

            return View(registerViewModel);
        }

        public ActionResult Logout()
        {
            return SignOut(new AuthenticationProperties() { RedirectUri = "Home/Index" }, "oidc", "cookie");
        }

        public ActionResult RegisterConfirmation()
        {
            return View();
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
