using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.ThirdParty.Helpers;
using WebApp.ThirdParty.ViewModels;

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

                client.SetBearerToken(token);

                Uri u = new Uri(ApiUrls.Companies.GetAll);
                IEnumerable<CompaniesViewModel> viewModelEnu = null;
                //HTTP POST
                var getCompanies = await client.GetAsync(u);
                string result = getCompanies.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var data = JsonConvert.DeserializeObject<IReadOnlyList<CompaniesViewModel>>(result);
                if (getCompanies.IsSuccessStatusCode)
                {
                    // var viewModel = JsonConvert.DeserializeObject<CompaniesViewModel>(result);                   
                    //  ModelState.AddModelError(string.Empty, "success");
                    return View(data.ToList());
                }
                if (getCompanies.IsSuccessStatusCode == false)
                {
                    ModelState.AddModelError(string.Empty, getCompanies.ReasonPhrase);
                    return View(viewModelEnu);
                }
                //postTask.Wait();


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
        public async Task<ActionResult> Create(CompaniesViewModel viewModel)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string token = string.Empty;

                    if (HttpContext.User.Identity is ClaimsIdentity identity)
                    {
                        token = User.FindFirst("AcessToken")?.Value;
                        var emailnew = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                    }
                    Uri u = new Uri(ApiUrls.Companies.Create);

                    var json = JsonConvert.SerializeObject(new { viewModel.CompanyName, viewModel.State });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    //var content = new FormUrlEncodedContent(new[]
                    //                {
                    //                    new KeyValuePair<string, string>("email",registerViewModel.email),
                    //                    new KeyValuePair<string, string>("password", registerViewModel.password)
                    //                  });
                    //HTTP POST


                    client.SetBearerToken(token);
                    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    client.Timeout = TimeSpan.FromMilliseconds(10000);
                    //HTTP POST
                    var postTask = await client.PostAsync(u, content);
                    //postTask.Wait();
                    string result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, result);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
