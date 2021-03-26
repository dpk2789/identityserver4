using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.MVC.Models;
using WebApp.MVC.Services;

namespace WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly ITokenService _tokenService;

        public HomeController(ILogger<HomeController> logger)
        {
            //_tokenService = tokenService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Weather()
        {
            using (var client = new HttpClient())
            {

                var result = client
                  .GetAsync("https://localhost:44300/weatherforecast")
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
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
