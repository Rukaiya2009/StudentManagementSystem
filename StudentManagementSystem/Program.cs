using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// 🟩 STEP 1: Configure SQL Server + EF Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🟩 STEP 2: Add Identity with Role Support
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Disable email confirmation
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); // Required for password reset/token-based features

// 🟩 STEP 3: Add MVC + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ⬇️ Build the app
var app = builder.Build();

// 🟩 STEP 4: Seed Roles (Admin, Student, Teacher)
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

    // ✅ Optional: Create default Admin user here (uncomment to activate)
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

// 🟩 STEP 5: Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint(); // Helpful during development
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Strict security in production
}

app.UseHttpsRedirection();
app.UseStaticFiles();        // ✅ Serve wwwroot (CSS, JS, images)

app.UseRouting();

app.UseAuthentication();     // ✅ Enable Identity login
app.UseAuthorization();      // ✅ Enforce [Authorize] attribute

// 🟩 STEP 6: Configure Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // ✅ Enable /Identity area

// 🟩 STEP 7: Start App
app.Run();
