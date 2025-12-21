using Microsoft.AspNetCore.Identity;

namespace ASP_Starbucks.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string RoleId { get; set; } = null!;

        [PersonalData]
        public string Name { get; set; } = null!;
        
        [PersonalData]
        public string Surname { get; set; } = null!;
        
        [PersonalData]
        public string Email { get; set; } = null!;
        
        [PersonalData]
        public string? Phone { get; set; }

        [PersonalData]
        public string? Address1 { get; set; }

        [PersonalData]
        public string? Address2 { get; set; }

        [PersonalData]
        public string? City { get; set; }

        [PersonalData]
        public string? State { get; set; }

        [PersonalData]
        public string? Zip { get; set; }

        public DateOnly? Birthdate { get; set; }
        public string Salt { get; set; } = null!;
        public string Dk { get; set; } = null!;
        public DateTime RegisteredAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public UserRole Role { get; set; } = null!;
    }
}
