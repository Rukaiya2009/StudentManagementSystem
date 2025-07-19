using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Services;
using StudentManagementSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// 🟩 STEP 1: Configure SQL Server + EF Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ✅ STEP 1.1: Fix Identity login path redirection
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Shared/AccessDenied";
    options.SlidingExpiration = true;
});

// 🟩 STEP 2: Add Identity with Role Support + Email Confirmation Required
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // ✅ Email confirmation REQUIRED
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 🟩 STEP 3: Register DevEmailSender for IEmailSender (for development/testing)
builder.Services.AddTransient<IEmailSender<IdentityUser>, DevEmailSender>();

// 🟩 STEP 4: Add MVC + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// 🟩 STEP 4.1: Configure AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

// 🟩 STEP 5: Seed Roles (Admin, Student, Teacher)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Student", "Teacher" };

    foreach (var role in roles)
    {
        var exists = await roleManager.RoleExistsAsync(role);
        if (!exists)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // ✅ Seed dummy teachers linked to departments
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!context.Teachers.Any())
    {
        var departments = context.Departments.ToList();
        if (departments.Count >= 3)
        {
            var teachers = new List<Teacher>
            {
                new Teacher { Name = "Farhana Haque", Email = "farhana@school.edu", DepartmentId = departments[0].DepartmentId },
                new Teacher { Name = "Tanvir Rahman", Email = "tanvir@school.edu", DepartmentId = departments[1].DepartmentId },
                new Teacher { Name = "Jahid Hasan", Email = "jahid@school.edu", DepartmentId = departments[2].DepartmentId },
            };
            context.Teachers.AddRange(teachers);
            context.SaveChanges();
        }
    }

    // ✅ Optional: Create default Admin user here (manual test only)
    /*
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var adminEmail = "admin@example.com";
    var adminPassword = "Admin@123";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newAdmin = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(newAdmin, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
    */
}

// 🟩 STEP 6: Middleware
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 🟩 STEP 7: Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Add route for AccessDenied page
app.MapControllerRoute(
    name: "accessDenied",
    pattern: "Shared/AccessDenied",
    defaults: new { controller = "Home", action = "AccessDenied" });

app.MapRazorPages();

app.Run();
