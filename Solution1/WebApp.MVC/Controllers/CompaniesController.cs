﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp.MVC.Controllers
{
    public class CompaniesController : Controller
    {
        // GET: CompaniesController
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                Uri u = new Uri("https://localhost:44363/api/Companies");
              
                //HTTP POST
                var getCompanies = await client.GetAsync(u);
                //postTask.Wait();
                string result = getCompanies.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (getCompanies.IsSuccessStatusCode)
                {
                    return RedirectToAction("RegisterConfirmation");
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