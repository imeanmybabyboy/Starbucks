using Microsoft.AspNetCore.Mvc;

namespace ASP_Starbucks.Models.User
{
    public class UserUpdateFormModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }

    }
}
