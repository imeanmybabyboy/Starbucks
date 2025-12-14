using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ASP_Starbucks.Models.User
{
    public class UserSigninFormModel
    {
        [FromForm(Name = "user-name-email")]
        [Required(ErrorMessage = "Enter an email/username")]
        public string Username { get; set; } = null!;

        [FromForm(Name = "user-password")]
        [Required(ErrorMessage = "Enter a password")]
        public string Password { get; set; } = null!;
    }
}
