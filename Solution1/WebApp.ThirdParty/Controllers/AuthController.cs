using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                                        new KeyValuePair<string, string>("grant_type",viewModel.Email),
                                        new KeyValuePair<string, string>("client_id", viewModel.Password),
                                         new KeyValuePair<string, string>("client_secret",viewModel.Email),
                                        new KeyValuePair<string, string>("scope", viewModel.Password),
                                         new KeyValuePair<string, string>("username",viewModel.Email),
                                        new KeyValuePair<string, string>("password", viewModel.Password)
                                      });
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

            return View(viewModel);
           
        }
    }
}
