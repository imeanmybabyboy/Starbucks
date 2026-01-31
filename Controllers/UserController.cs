using ASP_Starbucks.Data;
using ASP_Starbucks.Data.Entities;
using ASP_Starbucks.Middleware;
using ASP_Starbucks.Models.Responses;
using ASP_Starbucks.Models.User;
using ASP_Starbucks.Services.App;
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
    public class UserController : Controller
    {
        private readonly IAppService _appService;

        public UserController(IAppService appService, IKdfService kdfService, DataContext dataContext)
        {
            _appService = appService;
        }


        public async Task<JsonResult> ApiUpdate([FromBody] UserUpdateFormModel userFormModel)
        {
            var result = await _appService.UpdateUserAsync(userFormModel);

            return new JsonResult(result);
        }

        public async Task<IActionResult> ApiAuthenticateAsync()
        {
            var result = await _appService.AuthenticateAsync(Request.Headers.Authorization!, HttpContext.Session);

            return Json(result);
        }

        public JsonResult ApiLogout()
        {
            var result = _appService.Logout();

            return Json(result);
        }
    }
}
