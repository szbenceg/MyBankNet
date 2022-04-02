using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBank.Model;
using MyBank.Model.Dao;
using MyBank.Model.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddDbContext<CustomerContext>(options => 
    //options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<Customer, IdentityRole<int>>(options =>
    {
        // Password complexity
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = false;
        options.Password.RequiredUniqueChars = 3;

        // Wroung authentication configuration
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 10;
        options.Lockout.AllowedForNewUsers = true;

    
        options.User.RequireUniqueEmail = false;
    })
    .AddEntityFrameworkStores<CustomerContext>()
    .AddDefaultTokenProviders();


builder.Services.AddTransient<ICustomerService, CustomerServiceImp>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15); // max. 15 minute
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Authentication
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var serviceScope = app.Services.CreateScope())
{
    // Adatbázis inicializálása
    DbInitializer.Initialize(
        serviceScope.ServiceProvider);
}


app.Run();
