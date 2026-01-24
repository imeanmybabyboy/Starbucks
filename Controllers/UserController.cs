using ASP_Starbucks.Data;
using ASP_Starbucks.Data.Entities;
using ASP_Starbucks.Middleware;
using ASP_Starbucks.Models.User;
using ASP_Starbucks.Services.Hash;
using ASP_Starbucks.Services.Kdf;
using ASP_Starbucks.Services.Random;
using ASP_Starbucks.Services.Salt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASP_Starbucks.Controllers
{
    public class UserController(IKdfService kdfService, DataContext dataContext) : Controller
    {

        [HttpPatch]
        public async Task<JsonResult> ApiUpdate([FromBody] UserUpdateFormModel formModel)
        {
            try
            {
                bool isAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;

                if (!isAuthenticated)
                    return ErrorResponse("Unauthorized");

                string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
                var user = dataContext
                    .Users
                    .FirstOrDefault(u => u.Id == Guid.Parse(userId));

                if (user == null)
                    return ErrorResponse("User not found");

                bool hasAnyChanges = false;

                if (!string.IsNullOrWhiteSpace(formModel.Name))
                {
                    user.Name = formModel.Name;
                    hasAnyChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(formModel.Surname))
                {
                    user.Surname = formModel.Surname;
                    hasAnyChanges = true;
                }

                if (formModel.Phone != null)
                {
                    if (!string.IsNullOrWhiteSpace(formModel.Phone))
                    {
                        if (!IsPhoneValid(formModel.Phone, out string phoneError))
                            return ErrorResponse(phoneError);

                        user.Phone = formModel.Phone;
                    }
                    else
                    {
                        user.Phone = null; // if empty -> clear
                    }
                    hasAnyChanges = true;
                }

                if (formModel.Address1 != null)
                {
                    user.Address1 = string.IsNullOrWhiteSpace(formModel.Address1) ? null : formModel.Address1.Trim();
                    hasAnyChanges = true;
                }
                
                if (formModel.Address2 != null)
                {
                    user.Address2 = string.IsNullOrWhiteSpace(formModel.Address2) ? null : formModel.Address2.Trim();
                    hasAnyChanges = true;
                }

                if (formModel.City != null)
                {
                    user.City = string.IsNullOrWhiteSpace(formModel.City) ? null : formModel.City.Trim();
                    hasAnyChanges = true;
                }

                if (formModel.Zip != null)
                {
                    if (string.IsNullOrWhiteSpace(formModel.Zip))
                    {
                        user.Zip = null;
                        hasAnyChanges = true;
                    }
                    else
                    {
                        if (!Regex.IsMatch(formModel.Zip, @"^\d{5}(-\d{4})?$"))
                            return ErrorResponse("Invalid ZIP code format");

                        user.Zip = formModel.Zip;
                        hasAnyChanges = true;
                    }
                }

                if (!hasAnyChanges)
                    return OkResponse("No changes to save");

                AuthSessionMiddleware.SaveAuth(HttpContext, user); // session refresh

                await dataContext.SaveChangesAsync(); // database refresh

                return OkResponse("Profile updated sucessfully");
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex.Message);
            }
        }

        private bool IsPhoneValid(string phone, out string error)
        {
            error = string.Empty;

            if (!phone.StartsWith("+"))
            {
                error = "Phone has to start with '+' and the country code";
                return false;
            }

            if (!Regex.IsMatch(phone, @"^\+[1-9]\d{6,14}$")) // check phone number format
            {
                error = "Invalid phone number format";
                return false;
            }

            return true; // if valid (empty == valid)
        }

        private JsonResult ErrorResponse(string message)
        {
            return Json(new { Status = "error", Message = message });
        }

        private JsonResult OkResponse(string message)
        {
            return Json(new { Status = "ok", Message = message });
        }

        public async Task<User> _AuthenticateAsync()
        {
            string authHeader = Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                throw new UnauthorizedAccessException("Missing Authorization header");
            }

            string scheme = "Basic ";

            if (!authHeader.StartsWith(scheme))
                throw new UnauthorizedAccessException($"Invalid Authorization scheme");

            string basicCredentials = authHeader[scheme.Length..];

            if (basicCredentials.Length <= 3)
                throw new UnauthorizedAccessException($"Invalid or empty Credentials");

            string userPass;
            try
            {
                userPass = Encoding.UTF8.GetString(Convert.FromBase64String(basicCredentials));
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException($"Invalid Authorization format");
            }

            string[] parts = userPass.Split(':', 2);

            if (parts.Length != 2)
                throw new UnauthorizedAccessException($"Invalid {scheme}user-pass format.");

            string email = parts[0].ToLower().Trim();
            string password = parts[1];

            var user = await dataContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == email && u.DeletedAt == null);

            string dk = user != null ? kdfService.Dk(password, user.Salt)
                : kdfService.Dk(password, "dummy-salt-32-chars-long-xxxx");

            if (user == null || dk != user.Dk)
                throw new UnauthorizedAccessException("The email or password you entered is not valid. Please try again.");

            return user;
        }

        public async Task<IActionResult> ApiAuthenticateAsync()
        {
            try
            {
                // Check if user is in the session
                if (HttpContext.Session.Keys.Contains(AuthSessionMiddleware.SessionKey))
                {
                    var user = JsonSerializer.Deserialize<Data.Entities.User>(
                        HttpContext.Session.GetString(AuthSessionMiddleware.SessionKey)!)!;
                    return Json(new { Status = "Ok", User = user });
                }

                // If user is not in the session, then authenticate
                string authHeader = Request.Headers.Authorization.ToString();
                if (!string.IsNullOrEmpty(authHeader))
                {
                    var user = _AuthenticateAsync();
                    AuthSessionMiddleware.SaveAuth(HttpContext, await user);
                    return Json(new { Status = "Ok", User = user });
                }

                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return ErrorResponse("Unauthorized");

            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return ErrorResponse(ex.Message);
            }
        }

        public JsonResult ApiLogout()
        {
            try
            {
                bool isAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;

                if (!isAuthenticated)
                {
                    ErrorResponse("Unauthorized");
                }
                AuthSessionMiddleware.Logout(HttpContext);

                return OkResponse("Successfully logged out");
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex.Message);
            }
        }

        public async Task<IActionResult> AuthenticateAsync()
        {
            try
            {
                var user = _AuthenticateAsync();
                AuthSessionMiddleware.SaveAuth(HttpContext, await user);
                return Json(new { Status = "Ok", User = user });
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Content(ex.Message);
            }
        }
    }
}
