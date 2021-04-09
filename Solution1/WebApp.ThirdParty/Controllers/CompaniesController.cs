using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp.ThirdParty.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                string token = string.Empty;

                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                     token = User.FindFirst("AcessToken")?.Value;
                    var emailnew = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                }

                client
                  .SetBearerToken(token);

                Uri u = new Uri("https://localhost:44363/api/Companies");

                //HTTP POST
                var getCompanies = await client.GetAsync(u);
                //postTask.Wait();
                string result = getCompanies.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (getCompanies.IsSuccessStatusCode)
                {
                    return View();
                }
                if (getCompanies.IsSuccessStatusCode == false)
                {
                    return Challenge(new AuthenticationProperties() { RedirectUri = "Home/Index" }, "oidc");
                }
                ModelState.AddModelError(string.Empty, result);
            }
            return View();
        }

      
        public ActionResult Details(int id)
        {
            return View();
        }

       
        public ActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
