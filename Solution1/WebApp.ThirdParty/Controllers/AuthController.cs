using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.ThirdParty.Helpers;
using WebApp.ThirdParty.Models;
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
                    Uri u = new Uri(IdentityUrls.Identity.Login);

                    var content = new FormUrlEncodedContent(new[]
                                    {
                                        new KeyValuePair<string, string>("grant_type","password"),
                                        new KeyValuePair<string, string>("client_id", "mvc_thirdparty"),
                                         new KeyValuePair<string, string>("client_secret","secret"),
                                       // new KeyValuePair<string, string>("scope", "read"),
                                         new KeyValuePair<string, string>("scope", "read openid IdentityServer.Api.read"),
                                         new KeyValuePair<string, string>("username",viewModel.Email),
                                        new KeyValuePair<string, string>("password", viewModel.Password)
                                      });
                    //HTTP POST
                    var postTask = await client.PostAsync(u, content);
                    //postTask.Wait();
                    string result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var token = JsonConvert.DeserializeObject<Token>(result);

                    client.SetBearerToken(token.access_token);
                    //HTTP get user info
                    Uri userinfo = new Uri("https://localhost:44311/connect/userinfo");
                    var getUserInfo = await client.GetAsync(userinfo);
                  

                    string resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var data = JsonConvert.DeserializeObject<UserInfo>(resultuerinfo);


                    AuthenticationProperties authProperties = new AuthenticationProperties();

                    authProperties.AllowRefresh = true;
                    authProperties.IsPersistent = true;
                    authProperties.ExpiresUtc = DateTime.UtcNow.AddSeconds(int.Parse(token.expires_in));

                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Name, viewModel.Email),
                    new Claim("UserRoleClaim", data.role),
                    new Claim("AcessToken", string.Format("{0}", token.access_token)),
                    };                 

                    var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, result);
                }
            }

            return View(viewModel);

        }

        public IActionResult Register()
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
                    Uri u = new Uri(IdentityUrls.Identity.Register);

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

        public IActionResult RegisterConfirmation()
        {

            return View();
        }

        public IActionResult LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            foreach (var key in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Append(key, "", new CookieOptions() { Expires = DateTime.Now.AddDays(-1) });
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
