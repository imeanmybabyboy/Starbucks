using ASP_Starbucks.Data;
using ASP_Starbucks.Data.Entities;
using ASP_Starbucks.Exceptions;
using ASP_Starbucks.Middleware;
using ASP_Starbucks.Models.Responses;
using ASP_Starbucks.Models.User;
using ASP_Starbucks.Services.Kdf;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ASP_Starbucks.Services.App
{
    public class AppService : IAppService
    {
        private readonly DataContext _dataContext;
        private readonly IKdfService _kdfService;

        private const string UnauthorizedError = "Unauthorized action";
        private const string UserNotFoundError = "User not found";
        private const string NoChangesToSaveError = "No changes to save";
        private const string ChangesSavedWarning = "Changes were saved successfully";
        //private const string UnavailableHttpContextError = "HttpContext is not available";
        private const string MissingAuthorizationHeaderError = "Missing Authorization header";
        private const string InvalidAuthorizationSchemeError = "Invalid Authorization scheme";
        private const string CredentialsError = "Invalid or empty Credentials";
        private const string AuthorizationFormatError = "Invalid Authorization format";
        private const string InvalidUserPasswordFormat = "Invalid user-pass format";
        private const string InvalidCredentialsError = "The email or password you entered is not valid. Please try again";

        public AppService(DataContext dataContext, IKdfService kdfService)
        {
            _dataContext = dataContext;
            _kdfService = kdfService;
        }

        public async Task<ApiResponse> UpdateUserAsync(UserUpdateFormModel formModel, HttpContext httpContext)
        {
            try
            {
                if (!httpContext.User.Identity!.IsAuthenticated)
                    return ApiResponse.Error(UnauthorizedError);

                string userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)!.Value!;

                var user = _dataContext
                    .Users
                    .FirstOrDefault(u => u.Id == Guid.Parse(userId));

                if (user == null)
                    return ApiResponse.Error(UserNotFoundError);

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
                            return ApiResponse.Error(phoneError);

                        user.Phone = formModel.Phone;
                    }
                    else
                    {
                        user.Phone = null; // if empty -> clear
                    }
                    hasAnyChanges = true;
                }

                AddOptionalData(user, formModel);

                if (formModel.Zip != null)
                {
                    if (!string.IsNullOrWhiteSpace(formModel.Zip))
                    {
                        if (!IsZipValid(formModel.Zip, out string zipError))
                            return ApiResponse.Error(zipError);

                        user.Zip = formModel.Zip;
                    }
                    else
                    {
                        user.Zip = null; // if empty -> clear
                    }
                    hasAnyChanges = true;
                }

                if (!hasAnyChanges)
                    return ApiResponse.Ok(NoChangesToSaveError);

                AuthSessionMiddleware.SaveAuth(httpContext, user);
                await _dataContext.SaveChangesAsync();

                return ApiResponse.Ok(new UserProfileViewModel(user), ChangesSavedWarning);

            }
            catch (Exception ex)
            {
                return ApiResponse.Error(ex.Message);
            }
        }

        private bool IsPhoneValid(string phone, out string error)
        {
            error = string.Empty;
            bool isValid = Regex.IsMatch(phone, @"^\+[1-9]\d{6,14}$"); // check phone number format

            error = isValid ? string.Empty : "Invalid phone number format";
            return isValid;
        }

        private void AddOptionalData(User user, UserUpdateFormModel formModel)
        {
            user.Address1 = string.IsNullOrWhiteSpace(formModel.Address1) ? null : formModel.Address1.Trim();
            user.Address2 = string.IsNullOrWhiteSpace(formModel.Address2) ? null : formModel.Address2.Trim();
            user.City = string.IsNullOrWhiteSpace(formModel.City) ? null : formModel.City.Trim();
        }

        private bool IsZipValid(string zip, out string error)
        {

            error = string.Empty;
            bool isValid = Regex.IsMatch(zip, @"^\d{5}(-\d{4})?$"); // check zip format

            error = isValid ? string.Empty : "Invalid zip format";
            return isValid;
        }


        private Data.Entities.User GetUserFromSession(ISession session)
        {
            return JsonSerializer.Deserialize<Data.Entities.User>(
                        session.GetString(AuthSessionMiddleware.SessionKey)!)!;
        }

        private async Task<User> _AuthenticateAsync(string authHeader)
        {
            if (string.IsNullOrEmpty(authHeader))
            {
                throw new AuthorizationHeaderException(MissingAuthorizationHeaderError);
            }

            string scheme = "Basic ";

            if (!authHeader.StartsWith(scheme))
                throw new AuthorizationSchemeException(InvalidAuthorizationSchemeError);

            string basicCredentials = authHeader[scheme.Length..];

            if (basicCredentials.Length <= 3)
                throw new CredentialsException(CredentialsError);

            string userPass;

            try
            {
                userPass = Encoding.UTF8.GetString(Convert.FromBase64String(basicCredentials));
            }
            catch (Exception)
            {
                throw new AuthorizationFormatException(AuthorizationFormatError);
            }

            string[] parts = userPass.Split(':', 2);

            if (parts.Length != 2)
                throw new UsernamePasswordFormatException(InvalidUserPasswordFormat);

            string email = parts[0].ToLower().Trim();
            string password = parts[1];

            var user = await _dataContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == email && u.DeletedAt == null);

            string dk = user != null ? _kdfService.Dk(password, user.Salt)
                : _kdfService.Dk(password, "dummy-salt-32-chars-long-xxxx");

            if (user == null || dk != user.Dk)
                throw new CredentialsException(InvalidCredentialsError);

            return user;
        }

        public async Task<ApiResponse> ApiAuthenticateAsync(HttpContext httpContext, string authHeader, ISession session)
        {
            // Get user from the session
            if (session.Keys.Contains(AuthSessionMiddleware.SessionKey))

                return ApiResponse.Ok(GetUserFromSession(session));

            // If user is not in the session, then authenticate
            if (string.IsNullOrEmpty(authHeader))
                return ApiResponse.Error(UnauthorizedError);

            var user = await _AuthenticateAsync(authHeader);
            AuthSessionMiddleware.SaveAuth(httpContext, user);

            return ApiResponse.Ok(new UserProfileViewModel(user));
        }

        public ApiResponse ApiLogout(HttpContext httpContext)
        {
            try
            {
                bool isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;

                if (!isAuthenticated)
                {
                    ApiResponse.Error(UnauthorizedError);
                }
                AuthSessionMiddleware.Logout(httpContext);

                return ApiResponse.Ok();
            }
            catch (Exception ex)
            {
                return ApiResponse.Error(ex.Message);
            }
        }
    }
}
