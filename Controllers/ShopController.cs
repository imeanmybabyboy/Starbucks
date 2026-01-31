using ASP_Starbucks.Data;
using ASP_Starbucks.Models.Responses;
using ASP_Starbucks.Models.Shop;
using ASP_Starbucks.Services.App;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ASP_Starbucks.Controllers
{
    public class ShopController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IAppService _appService;

        public ShopController(IAppService appService, DataContext dataContext)
        {
            _dataContext = dataContext;
            _appService = appService;
        }

        [HttpGet]
        public JsonResult ApiIndex()
        {
            try
            {
                var menuCategories = _dataContext.Categories.ToList();

                return Json(ApiResponse.Ok(menuCategories));
            }
            catch (Exception ex)
            {
                return Json(ApiResponse.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ApiAddCategory([FromBody] ShopAddCategoryFormModel formModel)
        {
            var result = await _appService.AddCategory(formModel);

            return Json(result);
        }
    }
}
