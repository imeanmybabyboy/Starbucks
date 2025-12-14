using ASP_Starbucks.Data;
using ASP_Starbucks.Data.Entities;
using ASP_Starbucks.Middleware;
using ASP_Starbucks.Models.User;
using ASP_Starbucks.Services.Hash;
using ASP_Starbucks.Services.Kdf;
using ASP_Starbucks.Services.Random;
using ASP_Starbucks.Services.Salt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ASP_Starbucks.Controllers
{
    public class UserController(IKdfService kdfService, DataContext dataContext) : Controller
    {
        public IActionResult SignIn()
        {
            bool isAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            bool isWindowOpened = true;
            ViewData["isHeaderNavHidden"] = isWindowOpened;


            return View();
        }

        public IActionResult Personal()
        {
            bool isAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
                var user = dataContext.Users.Include(u => u.Role).First(u => u.Id == Guid.Parse(userId) && u.DeletedAt == null)!;
                return View(new UserProfileViewModel() { User = user, IsPersonal = true });
            }
        }

        public IActionResult Create()
        {
            bool isAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            bool isWindowOpened = true;
            ViewData["isHeaderNavHidden"] = isWindowOpened;

            return View();
        }

        public IActionResult ForgotPassword()
        {
            bool isWindowOpened = true;
            ViewData["isHeaderNavHidden"] = isWindowOpened;

            return View();
        }

        public JsonResult Authenticate()
        {
            // checking for authorization header existence
            string authHeader = Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Json(new
                {
                    status = "Error",
                    error = "Missing Authorization header"
                });
            }

            // checking correct scheme - "Basic " 
            string scheme = "Basic ";
            if (!authHeader.StartsWith(scheme))
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Json(new
                {
                    status = "Error",
                    error = $"Invalid Authorization scheme: {scheme}only"
                });
            }

            // checking basic credentials for being empty
            string basicCredentials = authHeader[scheme.Length..];
            if (basicCredentials.Length <= 3)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Json(new
                {
                    status = "Error",
                    error = $"Invalid or empty {scheme}Credentials"
                });
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
                return Json(new
                {
                    status = "Error",
                    error = $"Invalid {scheme}Credentials format: {ex.Message}"
                });
            }

            // divide credentials by ":" into 2 parts
            string[] parts = userPass.Split(':', 2);
            if (parts.Length != 2)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Json(new
                {
                    status = "Error",
                    error = $"Invalid {scheme}user-pass format: missing ':' separator"
                });
            }

            string login = parts[0];
            string password = parts[1];

            User user;
            try
            {
                user = dataContext.Users.FirstOrDefault(u => u.Email == login && u.DeletedAt == null)!;
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "Error",
                    error = ex.Message
                });
            }


            // checking for user presence in the db
            if (user == null)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Json(new
                {
                    status = "Error",
                    error = "The email or password you entered is not valid. Please try again."
                });
            }


            // checking for password correctness
            string dk = kdfService.Dk(password, user.Salt);
            if (dk != user.Dk)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Json(new
                {
                    status = "Error",
                    error = "The email or password you entered is not valid. Please try again."
                });
            }

            AuthSessionMiddleware.SaveAuth(HttpContext, user);

            return Json(new
            {
                status = "Ok",
                redirect = Url.Action("Index", "Home")
            });
        }
    }
}
