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

        public DbSet<Entities.Category> Categories { get; set; }
        public DbSet<Entities.Subcategory> Subcategories { get; set; }
        public DbSet<Entities.Product> Products { get; set; }

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


            modelBuilder.Entity<Entities.Subcategory>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(s => s.CategoryId);

            modelBuilder.Entity<Entities.Product>()
                .HasOne(p => p.Subcategory)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SubcategoryId);

            modelBuilder.Entity<Entities.Product>()
                .HasOne(p => p.Size)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SizeId);

            modelBuilder.Entity<Entities.Category>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            modelBuilder.Entity<Entities.Subcategory>()
                .HasIndex(s => s.Slug)
                .IsUnique();

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
                        Name = "Jack",
                        Surname = "Daniel",
                        Email = "jack.daniel@example.com",
                        Salt = userSalt,
                        Dk = userDk,
                        RegisteredAt = DateTime.MinValue,
                    }
                );


            // categories
            modelBuilder.Entity<Entities.Category>()
                .HasData(
                    new Entities.Category
                    {
                        Id = Guid.Parse("3D8682F6-942B-4D90-94E6-4D0F01535516"),
                        Name = "Fan Favorites",
                        Slug = "fan-favorites"
                    },
                    new Entities.Category
                    {
                        Id = Guid.Parse("07E75E04-8EF2-479C-BE21-6DB01B5E5781"),
                        Name = "Drinks",
                        Slug = "drinks"
                    },
                    new Entities.Category
                    {
                        Id = Guid.Parse("B936EFE2-4473-4E54-9EF5-170FB63F992B"),
                        Name = "Food",
                        Slug = "food"
                    },
                    new Entities.Category
                    {
                        Id = Guid.Parse("7E7BECF1-C635-41B7-B4A9-5AA9CBDACA90"),
                        Name = "At Home Coffee",
                        Slug = "at-home-coffee"
                    }
                );
        }
    }
}
