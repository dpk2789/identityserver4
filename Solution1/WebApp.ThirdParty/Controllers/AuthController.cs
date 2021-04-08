using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.ThirdParty.ViewModels;

namespace WebApp.ThirdParty.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    Uri u = new Uri("https://localhost:44311/connect/token");


                    var content = new FormUrlEncodedContent(new[]
                                    {
                                        new KeyValuePair<string, string>("grant_type","password"),
                                        new KeyValuePair<string, string>("client_id", "mvc_thirdparty"),
                                         new KeyValuePair<string, string>("client_secret","secret"),
                                        new KeyValuePair<string, string>("scope", "read"),
                                         new KeyValuePair<string, string>("username","bob"),
                                        new KeyValuePair<string, string>("password", "Pass123$")
                                      });
                    //HTTP POST
                    var postTask = await client.PostAsync(u, content);
                    //postTask.Wait();
                    string result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var token = JsonConvert.DeserializeObject<Token>(result);

                    AuthenticationProperties authProperties = new AuthenticationProperties();

                    authProperties.AllowRefresh = true;
                    authProperties.IsPersistent = true;
                    authProperties.ExpiresUtc = DateTime.UtcNow.AddSeconds(int.Parse(token.expires_in));

                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Name, viewModel.Email),
                    new Claim("AcessToken", string.Format("Bearer {0}", token.access_token)),
                    };

                    var identity = new ClaimsIdentity(claims, "ApplicationCookie");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("RegisterConfirmation");
                    }

                    ModelState.AddModelError(string.Empty, result);
                }
            }

            return View(viewModel);

        }
    }
}
