using ASP_Starbucks.Services.Kdf;
using Microsoft.EntityFrameworkCore;

namespace ASP_Starbucks.Data
{
    public class DataContext : DbContext
    {
        private readonly IKdfService _kdfService;


        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.UserRole> UserRoles { get; set; }
        public DbSet<Entities.Token> Tokens { get; set; }

        public DataContext(DbContextOptions options, IKdfService kdfService) : base(options)
        {
            _kdfService = kdfService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Entities.User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Entities.Token>()
                .HasOne(t => t.User)
                .WithMany();


            ////// Seeding
            modelBuilder.Entity<Entities.UserRole>()
                .HasData(
                    new Entities.UserRole
                    {
                        Id = "Admin",
                        Description = "Full access role",
                        CanCreate = 1,
                        CanDelete = 1,
                        CanRead = 1,
                        CanUpdate = 1
                    },

                    new Entities.UserRole
                    {
                        Id = "User",
                        Description = "Self registered user",
                        CanCreate = 0,
                        CanDelete = 0,
                        CanRead = 0,
                        CanUpdate = 0
                    }
                );

            string adminSalt = "10F3348842EC";
            string adminDk = _kdfService.Dk("Admin", adminSalt);
            modelBuilder.Entity<Entities.User>()
                .HasData(
                    new Entities.User
                    {
                        Id = Guid.Parse("869B525F-B7E7-4A00-9313-10F3348842EC"),
                        RoleId = "Admin",
                        Name = "Administrator",
                        Surname = "Administrator",
                        Email = "admin@change.me",
                        Salt = adminSalt,
                        Dk = adminDk,
                        RegisteredAt = DateTime.MinValue,
                    }
                );


            string userSalt = "D7235525F88F";
            string userDk = _kdfService.Dk("User", userSalt);
            modelBuilder.Entity<Entities.User>()
                .HasData(
                    new Entities.User
                    {
                        Id = Guid.Parse("C588EFF8-359A-4C25-949E-D7235525F88F"),
                        RoleId = "User",
                        Name = "UserName",
                        Surname = "UserSurname",
                        Email = "user@example.com",
                        Salt = userSalt,
                        Dk = userDk,
                        RegisteredAt = DateTime.MinValue,
                    }
                );
        }
    }
}
