using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PropertyRentalManagement.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Set up Authentication to use cookie service
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "PropertyRentalManagement";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Unauthorized";
});

var connection = builder.Configuration.GetConnectionString("DBConnect");
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connection));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Seed database
AppDbInitializer.Seed(app);

app.Run();
