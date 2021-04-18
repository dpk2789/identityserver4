using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.MVC.Services;

namespace WebApp.MVC.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ITokenService _tokenService;

        public CompaniesController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                var tokenResponse = await _tokenService.GetToken("read");

                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    var principal = new ClaimsPrincipal(identity);                    
                    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var email = User.FindFirst("sub")?.Value;
                    string email1 = User.FindFirst(ClaimTypes.Name)?.Value;
                    var emailnew = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                }
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId1 = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                client
                  .SetBearerToken(tokenResponse.AccessToken);

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

        // GET: CompaniesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompaniesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompaniesController/Create
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

        // GET: CompaniesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompaniesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: CompaniesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompaniesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
