using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.RazorPages.Helpers;
using WebApp.RazorPages.Models;

namespace WebApp.RazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginViewModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
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
                                         new KeyValuePair<string, string>("username",Input.Email),
                                        new KeyValuePair<string, string>("password", Input.Password)
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
                    new Claim(ClaimTypes.Name, Input.Email),
                    new Claim("UserRoleClaim", data.role),
                    new Claim("AcessToken", string.Format("{0}", token.access_token)),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToPage("/Index");
                    }

                    ModelState.AddModelError(string.Empty, result);
                }
            }

            return Page();
        }
    }

    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
