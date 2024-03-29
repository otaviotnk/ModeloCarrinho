﻿using Carrinho.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Carrinho.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        { }

        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
