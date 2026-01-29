using Microsoft.AspNetCore.Identity;

namespace ASP_Starbucks.Models.User
{
    public class UserProfileViewModel
    {
        public string RoleId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public DateOnly? Birthdate { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public UserProfileViewModel(Data.Entities.User user)
        {
            RoleId = user.RoleId;
            Name = user.Name;
            Surname = user.Surname;
            Email = user.Email;
            Phone = user.Phone;
            Address1 = user.Address1;
            Address2 = user.Address2;
            City = user.City;
            State = user.State;
            Zip = user.Zip;
            Birthdate = user.Birthdate;
            RegisteredAt = user.RegisteredAt;
            DeletedAt = user.DeletedAt;
        }

    }
}
