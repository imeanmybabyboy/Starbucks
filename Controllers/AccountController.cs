using Microsoft.AspNetCore.Mvc;

namespace ASP_Starbucks.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SignIn()
        {
            bool isWindowOpened = true;
            ViewData["isWindowOpened"] = isWindowOpened;

            return View();
        }
    }
}
