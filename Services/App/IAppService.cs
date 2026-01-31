using ASP_Starbucks.Models.Responses;
using ASP_Starbucks.Models.Shop;
using ASP_Starbucks.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Starbucks.Services.App
{
    public interface IAppService
    {
        public Task<ApiResponse> UpdateUserAsync (UserUpdateFormModel model);
        public Task<ApiResponse> AuthenticateAsync(string authHeader, ISession session);
        public ApiResponse Logout();
        public Task<ApiResponse> AddCategory(ShopAddCategoryFormModel formModel);
        //public Task<ApiResponse> GetMenu();
    }
}
