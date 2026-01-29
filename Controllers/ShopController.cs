using ASP_Starbucks.Data;
using ASP_Starbucks.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ASP_Starbucks.Controllers
{
    public class ShopController(DataContext dataContext) : Controller
    {
        [HttpGet]
        public JsonResult ApiIndex()
        {
            try
            {
                var menuCategories = dataContext.Categories.ToList();

                return Json(ApiResponse.Ok(menuCategories));
            }
            catch (Exception ex)
            {
                return Json(ApiResponse.Error(ex.Message));
            }
        }
    }
}
