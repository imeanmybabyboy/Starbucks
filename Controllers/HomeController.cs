using System.Diagnostics;
using ASP_Starbucks.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Starbucks.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Menu()
        {
            return View();
        }
        
        public IActionResult Rewards()
        {
            return View();
        }
        
        public IActionResult GiftCards()
        {
            return View();
        }

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
