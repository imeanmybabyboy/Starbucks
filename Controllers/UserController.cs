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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

                ICollection<int> days = new List<int>();
                int maxDays = 31;
                for (int i = 1; i <= maxDays; i++)
                {
                    days.Add(i);
                }

                ICollection<string> months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

                ICollection<string> states = new List<string>()
                {
                    "Alabama",
                    "Alaska",
                    "Arizona",
                    "Arkansas",
                    "California",
                    "Colorado",
                    "Connecticut",
                    "Delaware",
                    "Florida",
                    "Georgia",
                    "Hawaii",
                    "Idaho",
                    "Illinois",
                    "Indiana",
                    "Iowa",
                    "Kansas",
                    "Kentucky",
                    "Louisiana",
                    "Maine",
                    "Maryland",
                    "Massachusetts",
                    "Michigan",
                    "Minnesota",
                    "Mississippi",
                    "Missouri",
                    "Montana",
                    "Nebraska",
                    "Nevada",
                    "New Hampshire",
                    "New Jersey",
                    "New Mexico",
                    "New York",
                    "North Carolina",
                    "North Dakota",
                    "Ohio",
                    "Oklahoma",
                    "Oregon",
                    "Pennsylvania",
                    "Rhode Island",
                    "South Carolina",
                    "South Dakota",
                    "Tennessee",
                    "Texas",
                    "Utah",
                    "Vermont",
                    "Virginia",
                    "Washington",
                    "West Virginia",
                    "Wisconsin",
                    "Wyoming"
                };

                UserProfileViewModel profileViewModel = new()
                {
                    User = user,
                    IsPersonal = true,
                    Days = days,
                    Months = months,
                    States = states,
                };

                return View(profileViewModel);
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

        [HttpPatch]
        public async Task<JsonResult> Update([FromBody] UserUpdateFormModel formModel)
        {

            bool isAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;

            if (!isAuthenticated)
            {
                return Json(new
                {
                    Status = "error",
                    Message = "Unauthorized"
                });
            }

            string userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
            var user = dataContext
                .Users
                .First(u => u.Id == Guid.Parse(userId));

            if (user == null)
            {
                return Json(new
                {
                    Status = "error",
                    Message = "Invalid user restoring from identity"
                });
            }

            Type userType = user.GetType();

            // check if at least one field is present
            bool isAllNulls = true;

            foreach (var prop in formModel.GetType().GetProperties())
            {
                object? val = prop.GetValue(formModel, null);

                if (val != null)
                {
                    isAllNulls = false;

                    if (prop.Name == "Phone")
                    {
                        var userProp = userType.GetProperty(prop.Name);

                        if (userProp != null)
                        {
                            if (formModel.Phone != "")
                            {
                                if (!Regex.IsMatch(formModel.Phone!, @"\++") && formModel.Phone != "")
                                {
                                    return Json(new
                                    {
                                        Status = "error",
                                        Message = "Phone number should start from '+' + country code"
                                    });
                                }
                                else if (!string.IsNullOrEmpty(formModel.Phone) && formModel.Phone.Length < 2)
                                {
                                    return Json(new
                                    {
                                        Status = "error",
                                        Message = "Phone number cannot be that short"
                                    });
                                }
                                else if (!Regex.IsMatch(formModel.Phone!, @"^\+?[0-9]+$") && formModel.Phone != "")
                                {
                                    return Json(new
                                    {
                                        Status = "error",
                                        Message = "Phone number should contain only numbers"
                                    });
                                }
                            }

                            if (formModel.Phone == "")
                            {
                                userProp.SetValue(user, null);
                            }
                            else
                            {
                                userProp.SetValue(user, val);
                            }
                        }
                    }
                    else
                    {
                        var userProp = userType.GetProperty(prop.Name);

                        if (val != null)
                        {
                            if (val.ToString() == "")
                            {
                                userProp!.SetValue(user, null);
                            }
                            else if (userProp != null)
                            {
                                userProp.SetValue(user, val);
                            }
                            else
                            {
                                return Json(new
                                {
                                    Status = "ok",
                                    Message = $"Form property '{prop.Name}' was not found in identity '{userType.Name}'"
                                });
                            }
                        }
                    }
                }
            }

            if (isAllNulls)
            {
                return Json(new
                {
                    Status = "ok",
                    Message = "All nulls"
                });
            }

            var saveTask = dataContext.SaveChangesAsync();
            AuthSessionMiddleware.SaveAuth(HttpContext, user);

            await saveTask;

            return Json(new
            {
                Status = "ok",
                Message = "Changes were saved"
            });
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
