using ASP_Starbucks.Data;
using ASP_Starbucks.Middleware;
using ASP_Starbucks.Services.Hash;
using ASP_Starbucks.Services.Kdf;
using ASP_Starbucks.Services.Random;
using ASP_Starbucks.Services.Salt;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Custom services
builder.Services.AddRandom();
builder.Services.AddSalt();
builder.Services.AddHash();
builder.Services.AddKdf();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(1000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Sessions
builder.Services.AddDistributedMemoryCache();


string connectionString = builder.Configuration.GetConnectionString("StarbucksDb") ?? throw new FileNotFoundException("Connection String Configuration: key not found: StarbucksDb");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.UseSession();


// Custom middleware
app.UseAuthSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
