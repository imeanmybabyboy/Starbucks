using ASP_Starbucks.Models.Responses;
using ASP_Starbucks.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Starbucks.Services.App
{
    public interface IAppService
    {
        public Task<ApiResponse> UpdateUserAsync (UserUpdateFormModel model, HttpContext httpContext);
        public Task<ApiResponse> ApiAuthenticateAsync(HttpContext httpContext, string authHeader, ISession session);
        public ApiResponse ApiLogout(HttpContext httpContext);
    }
}
