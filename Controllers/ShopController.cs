using ASP_Starbucks.Data;
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
                
                return Json(new
                {
                    Status = "Ok",
                    data = menuCategories
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = "Error",
                    Error = ex.Message
                });
            }
        }
    }
}
