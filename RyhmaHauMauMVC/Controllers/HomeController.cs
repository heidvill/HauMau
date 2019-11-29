using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RyhmaHauMauMVC.Models;
using RyhmaHauMauMVC.Extensions;

namespace RyhmaHauMauMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Muutokset = FormDataHelper.Muutokset();
            ViewBag.Tulossa = FormDataHelper.Tulossa();

            return View();
        }

        public ActionResult GPS()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GPS(string address)
        {
            ViewBag.address = address;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
