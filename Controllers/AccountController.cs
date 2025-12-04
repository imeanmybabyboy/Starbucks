using ASP_Starbucks.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Text.Json;

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

        public IActionResult Create()
        {
            bool isWindowOpened = true;
            ViewData["isWindowOpened"] = isWindowOpened;

            return View();
        }

        public IActionResult ForgotPassword()
        {
            bool isWindowOpened = true;
            ViewData["isWindowOpened"] = isWindowOpened;

            return View();
        }

        public IActionResult Authenticate()
        {
            // checking for authorization header existence
            string authHeader = Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Content("Missing Authorization header");
            }

            // checking correct scheme - "Basic " 
            string scheme = "Basic ";
            if (!authHeader.StartsWith(scheme))
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Content($"Invalid Authorization scheme: {scheme}only");
            }

            // checking basic credentials for being empty
            string basicCredentials = authHeader[scheme.Length..];
            if (basicCredentials.Length <= 3)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Content($"Invalid or empty {scheme}Credentials");
            }

            // decoding credentials from Base64
            string userPass;
            try
            {
                userPass = Encoding.UTF8.GetString(Convert.FromBase64String(basicCredentials));
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Content($"Invalid {scheme}Credentials format: {ex.Message}");
            }

            // divide credentials by ":" into 2 parts
            string[] parts = userPass.Split(':', 2);
            if (parts.Length != 2)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Content($"Invalid {scheme}user-pass format: missing ':' separator");
            }

            string login = parts[0];
            string password = parts[1];

            // create database and follow the instructions

            return Content("Everything is fine");
        }
    }
}
