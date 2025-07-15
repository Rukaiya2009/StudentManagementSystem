# 📚 Student Management System - API Documentation

## Table of Contents
1. [Overview](#overview)
2. [Architecture](#architecture)
3. [Data Models](#data-models)
4. [Controllers & APIs](#controllers--apis)
5. [Services](#services)
6. [Views & Components](#views--components)
7. [Authentication & Authorization](#authentication--authorization)
8. [Usage Examples](#usage-examples)
9. [File Upload System](#file-upload-system)
10. [Configuration](#configuration)

---

## 🔍 Overview

The Student Management System is a comprehensive web application built with ASP.NET Core MVC that provides role-based access control for managing educational institutions. The system supports three primary user roles: **Admin**, **Teacher**, and **Student**.

### Key Features
- ✅ **CRUD Operations**: Complete Create, Read, Update, Delete functionality for all entities
- ✅ **Role-Based Access Control**: Different permissions for Admin, Teacher, and Student roles
- ✅ **File Upload**: Profile picture management with validation
- ✅ **Grade Management**: Grade assignment and GPA calculation
- ✅ **Email Confirmation**: User registration with email verification
- ✅ **Responsive UI**: Bootstrap-based responsive design with dark/light mode support

---

## 🏗️ Architecture

### Project Structure
```
StudentManagementSystem/
├── Controllers/           # MVC Controllers (API endpoints)
├── Models/               # Data models and entities
├── Views/                # Razor views (UI components)
├── Services/             # Business logic services
├── Data/                 # Entity Framework DbContext
├── wwwroot/              # Static files (CSS, JS, images)
└── Areas/Identity/       # ASP.NET Identity pages
```

### Technology Stack
- **Framework**: ASP.NET Core MVC (.NET 6+)
- **ORM**: Entity Framework Core
- **Database**: SQL Server / LocalDB
- **Authentication**: ASP.NET Core Identity
- **UI Framework**: Bootstrap 5
- **Icons**: Font Awesome 6.4.0

---

## 📊 Data Models

### 1. Student Model
```csharp
public class Student
{
    public int StudentId { get; set; }           // Primary Key
    
    [Required, StringLength(100)]
    public string FullName { get; set; }         // Student's full name
    
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }   // Birth date
    
    [Required]
    public string Gender { get; set; }           // Gender
    
    [EmailAddress]
    public string? Email { get; set; }           // Email address
    
    public string Phone { get; set; }            // Phone number
    public string? Address { get; set; }         // Home address
    public string? ProfilePicture { get; set; }  // Profile image filename
    
    // Navigation Properties
    public ICollection<Enrollment> Enrollments { get; set; }
}
```

**Usage Example:**
```csharp
var student = new Student
{
    FullName = "John Doe",
    Email = "john.doe@email.com",
    Gender = "Male",
    DateOfBirth = new DateTime(2000, 5, 15)
};
```

### 2. Teacher Model
```csharp
public class Teacher
{
    public int TeacherId { get; set; }           // Primary Key
    
    [Required]
    public string Name { get; set; }             // Teacher's name
    
    [EmailAddress]
    public string Email { get; set; }            // Email address
    
    public string DepartmentName { get; set; }   // Associated department
    
    // Navigation Properties
    public ICollection<Course> Courses { get; set; }
}
```

### 3. Course Model
```csharp
public class Course
{
    public int CourseId { get; set; }            // Primary Key
    
    [Required]
    public string CourseName { get; set; }       // Course name
    
    [Required, StringLength(20)]
    public string CourseCode { get; set; }       // Course code (e.g., "CS101")
    
    public int Credit { get; set; }              // Credit hours
    public int Credits { get; set; }             // Additional credits field
    
    // Foreign Keys
    public int DepartmentId { get; set; }
    public int? TeacherId { get; set; }          // Optional teacher assignment
    
    // Navigation Properties
    public Department Department { get; set; }
    public Teacher Teacher { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
}
```

### 4. Department Model
```csharp
public class Department
{
    public int DepartmentId { get; set; }        // Primary Key
    
    [Required]
    public string DepartmentName { get; set; }   // Department name
    
    public string? Description { get; set; }     // Department description
    
    // Navigation Properties
    public ICollection<Course> Courses { get; set; }
}
```

### 5. Enrollment Model
```csharp
public class Enrollment
{
    public int EnrollmentId { get; set; }        // Primary Key
    
    // Foreign Keys
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    
    [StringLength(10)]
    public string Grade { get; set; }            // Grade (A, B, C, D, F)
    
    public DateTime EnrollmentDate { get; set; } // Enrollment date
    
    // Navigation Properties
    public Student Student { get; set; }
    public Course Course { get; set; }
}
```

---

## 🎮 Controllers & APIs

### 1. StudentsController

**Base Route**: `/Students`
**Authorization**: Admin, Teacher roles required

#### GET Endpoints

##### `GET /Students` - List All Students
```csharp
[Authorize(Roles = "Admin,Teacher")]
public async Task<IActionResult> Index()
```
- **Description**: Retrieves all students for display in a table
- **Returns**: View with `IEnumerable<Student>`
- **Authorization**: Admin, Teacher

##### `GET /Students/Details/{id}` - Get Student Details
```csharp
[Authorize(Roles = "Admin,Teacher,Student")]
public async Task<IActionResult> Details(int? id)
```
- **Parameters**: `id` - Student ID
- **Returns**: Student details view or NotFound
- **Authorization**: Admin, Teacher, Student

##### `GET /Students/Create` - Show Create Form
```csharp
[Authorize(Roles = "Admin,Teacher")]
public IActionResult Create()
```
- **Returns**: Create student form view
- **Authorization**: Admin, Teacher

##### `GET /Students/Edit/{id}` - Show Edit Form
```csharp
[Authorize(Roles = "Admin,Teacher")]
public async Task<IActionResult> Edit(int? id)
```
- **Parameters**: `id` - Student ID
- **Returns**: Edit form pre-populated with student data
- **Authorization**: Admin, Teacher

#### POST Endpoints

##### `POST /Students/Create` - Create New Student
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
[Authorize(Roles = "Admin,Teacher")]
public async Task<IActionResult> Create(Student student, IFormFile ProfileImage)
```
- **Parameters**: 
  - `student` - Student model data
  - `ProfileImage` - Optional profile image file
- **File Validation**:
  - **Allowed Types**: JPEG, JPG, PNG, GIF
  - **Max Size**: 5MB
  - **Auto-generated filename**: GUID + original extension
- **Returns**: Redirect to Index on success, or form with validation errors
- **Authorization**: Admin, Teacher

**Usage Example:**
```html
<form asp-action="Create" enctype="multipart/form-data" method="post">
    <input asp-for="FullName" />
    <input asp-for="Email" />
    <input type="file" name="ProfileImage" accept="image/*" />
    <button type="submit">Create Student</button>
</form>
```

### 2. TeachersController

**Base Route**: `/Teachers`
**Authorization**: Various based on action

#### Key Endpoints

##### `GET /Teachers` - List All Teachers
```csharp
[Authorize(Roles = "Admin,Teacher")]
public async Task<IActionResult> Index()
```

##### `POST /Teachers/Create` - Create New Teacher
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Create(Teacher teacher)
```

### 3. CoursesController

**Base Route**: `/Courses`

#### Key Features
- **Public Access**: Course listing is accessible to all users
- **Role-Based Actions**: Create/Edit/Delete restricted to Admin and Teacher
- **Department Integration**: Courses are linked to departments

##### `GET /Courses` - List All Courses
```csharp
public async Task<IActionResult> Index()
```
- **Authorization**: Public access
- **Returns**: All courses with department and teacher information

##### `POST /Courses/Create` - Create New Course
```csharp
[Authorize(Roles = "Admin,Teacher")]
public async Task<IActionResult> Create(Course course)
```
- **Authorization**: Admin, Teacher only

### 4. DepartmentsController

**Base Route**: `/Departments`
**Authorization**: Admin, Teacher roles

### 5. EnrollmentsController

**Base Route**: `/Enrollments`
**Features**: Grade management, student-course relationships

---

## 🔧 Services

### 1. DevEmailSender Service

**Interface**: `IEmailSender<IdentityUser>`
**Purpose**: Development email service for user confirmation

```csharp
public class DevEmailSender : IEmailSender<IdentityUser>
{
    public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink)
    public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink)
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
}
```

**Features:**
- **Auto-opens confirmation links** in development
- **Console logging** of all email content
- **Cross-platform support** for link opening

**Usage in Program.cs:**
```csharp
builder.Services.AddTransient<IEmailSender<IdentityUser>, DevEmailSender>();
```

---

## 🎨 Views & Components

### Enhanced Table Views

All index views now include:

#### 1. **Fixed Header Visibility**
```html
<thead class="table-dark">
    <tr>
        <th class="text-white">Column Name</th>
    </tr>
</thead>
```
- **Dark headers** with white text for visibility in all modes
- **Consistent contrast** across light and dark themes

#### 2. **Icon-Enhanced Action Buttons**
```html
<div class="btn-group" role="group">
    <a asp-action="Details" asp-route-id="@item.Id" 
       class="btn btn-info btn-sm" title="View Details">
        <i class="fas fa-eye"></i>
    </a>
    <a asp-action="Edit" asp-route-id="@item.Id" 
       class="btn btn-warning btn-sm" title="Edit">
        <i class="fas fa-edit"></i>
    </a>
    <a asp-action="Delete" asp-route-id="@item.Id" 
       class="btn btn-danger btn-sm" title="Delete"
       onclick="return confirm('Are you sure?')">
        <i class="fas fa-trash"></i>
    </a>
</div>
```

#### 3. **Empty State Handling**
```html
@if (!Model.Any())
{
    <div class="text-center py-5">
        <i class="fas fa-users fa-3x text-muted mb-3"></i>
        <h4 class="text-muted">No records found</h4>
        <p class="text-muted">Get started by adding your first record.</p>
        <a asp-action="Create" class="btn btn-primary">
            <i class="fas fa-plus"></i> Add First Record
        </a>
    </div>
}
```

### View Components Features

#### Students View (`/Views/Students/Index.cshtml`)
- **Profile Picture Display**: Shows thumbnail or default icon
- **Responsive Cards**: Wrapped in Bootstrap cards with shadows
- **Action Confirmation**: Delete confirmation dialogs

#### Courses View (`/Views/Courses/Index.cshtml`)
- **Credit Badges**: Visual indicators for course credits
- **Department Badges**: Color-coded department labels
- **Role-Based Actions**: Actions shown based on user permissions

#### Enrollments View (`/Views/Enrollments/Index.cshtml`)
- **Grade Color Coding**:
  ```html
  <span class="badge 
      @(item.Grade == "A" ? "bg-success" :
        item.Grade == "B" ? "bg-primary" :
        item.Grade == "C" ? "bg-warning" :
        item.Grade == "D" ? "bg-info" :
        item.Grade == "F" ? "bg-danger" : "bg-secondary")">
      @item.Grade
  </span>
  ```

---

## 🔐 Authentication & Authorization

### Role-Based Access Control

#### Roles Hierarchy
1. **Admin**: Full system access
2. **Teacher**: Student and course management
3. **Student**: Limited to own profile and course viewing

#### Implementation Example
```csharp
[Authorize(Roles = "Admin,Teacher")]
public async Task<IActionResult> ManageStudents()

[Authorize(Roles = "Admin")]
public async Task<IActionResult> SystemSettings()

[Authorize(Roles = "Student")]
public async Task<IActionResult> MyDashboard()
```

### Identity Configuration
```csharp
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // Email confirmation required
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
```

---

## 📝 Usage Examples

### 1. Creating a New Student with Profile Picture

**Controller Action:**
```csharp
[HttpPost]
public async Task<IActionResult> Create(Student student, IFormFile ProfileImage)
{
    if (ModelState.IsValid)
    {
        if (ProfileImage != null && ProfileImage.Length > 0)
        {
            // Validate file type and size
            string fileName = await SaveProfileImage(ProfileImage);
            student.ProfilePicture = fileName;
        }
        
        _context.Add(student);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Student created successfully!";
        return RedirectToAction(nameof(Index));
    }
    return View(student);
}
```

**View Form:**
```html
<form asp-action="Create" enctype="multipart/form-data" method="post">
    <div class="form-group">
        <label asp-for="FullName"></label>
        <input asp-for="FullName" class="form-control" />
        <span asp-validation-for="FullName" class="text-danger"></span>
    </div>
    
    <div class="form-group">
        <label for="ProfileImage">Profile Picture</label>
        <input type="file" name="ProfileImage" class="form-control" accept="image/*" />
    </div>
    
    <button type="submit" class="btn btn-primary">Create Student</button>
</form>
```

### 2. Enrolling a Student in a Course

**Controller Logic:**
```csharp
public async Task<IActionResult> EnrollStudent(int studentId, int courseId)
{
    var enrollment = new Enrollment
    {
        StudentId = studentId,
        CourseId = courseId,
        EnrollmentDate = DateTime.Now,
        Grade = "N/A" // To be assigned later
    };
    
    _context.Enrollments.Add(enrollment);
    await _context.SaveChangesAsync();
    
    return RedirectToAction("Details", "Students", new { id = studentId });
}
```

### 3. Grade Assignment with Validation

**Model Validation:**
```csharp
[StringLength(10)]
[RegularExpression(@"^[A-F]$|^N/A$", ErrorMessage = "Grade must be A, B, C, D, F, or N/A")]
public string Grade { get; set; } = "N/A";
```

### 4. Role-Based Navigation

**Layout Navigation:**
```html
@if (SignInManager.IsSignedIn(User))
{
    @if (User.IsInRole("Admin"))
    {
        <li class="nav-item">
            <a class="nav-link" asp-controller="Admin" asp-action="Dashboard">
                <i class="fas fa-tachometer-alt"></i> Admin Dashboard
            </a>
        </li>
    }
    
    @if (User.IsInRole("Teacher"))
    {
        <li class="nav-item">
            <a class="nav-link" asp-controller="Students" asp-action="Index">
                <i class="fas fa-users"></i> Manage Students
            </a>
        </li>
    }
}
```

---

## 📁 File Upload System

### Configuration
- **Upload Directory**: `wwwroot/images/`
- **Allowed File Types**: JPEG, JPG, PNG, GIF
- **Maximum File Size**: 5MB
- **File Naming**: GUID + original extension

### Implementation
```csharp
private async Task<string> SaveProfileImage(IFormFile file)
{
    var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
    
    if (!allowedTypes.Contains(file.ContentType.ToLower()))
        throw new InvalidOperationException("Invalid file type");
        
    if (file.Length > 5 * 1024 * 1024)
        throw new InvalidOperationException("File too large");
    
    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);
    
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }
    
    return fileName;
}
```

---

## ⚙️ Configuration

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StudentManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Application Settings
```json
{
  "AppSettings": {
    "MaxFileSize": "5242880",
    "AllowedImageTypes": ["image/jpeg", "image/png", "image/gif"],
    "DefaultProfileImage": "/images/default-profile.png"
  }
}
```

### Startup Configuration
```csharp
// Program.cs key configurations
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Shared/AccessDenied";
    options.SlidingExpiration = true;
});
```

---

## 🚀 Getting Started

### Prerequisites
- .NET 6.0 or later
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 (recommended)

### Setup Steps

1. **Clone the repository**
```bash
git clone <repository-url>
cd StudentManagementSystem
```

2. **Restore packages**
```bash
dotnet restore
```

3. **Update database**
```bash
dotnet ef database update
```

4. **Run the application**
```bash
dotnet run
```

5. **Access the application**
   - Navigate to `https://localhost:5001`
   - Register as a new user
   - Admin will need to assign roles through the database initially

---

## 📈 Performance Tips

1. **Use async/await** for all database operations
2. **Include related data** efficiently with EF Core Include()
3. **Implement pagination** for large datasets
4. **Use ViewModels** to reduce data transfer
5. **Enable response caching** for static content

---

## 🔍 Troubleshooting

### Common Issues

1. **Database Connection Errors**
   - Verify connection string in appsettings.json
   - Ensure SQL Server is running
   - Run `dotnet ef database update`

2. **File Upload Issues**
   - Check file permissions on wwwroot/images/
   - Verify file size and type restrictions
   - Ensure proper form encoding: `enctype="multipart/form-data"`

3. **Authorization Issues**
   - Verify user roles in AspNetUserRoles table
   - Check [Authorize] attributes on controllers
   - Ensure user is properly logged in

---

This documentation provides comprehensive coverage of all public APIs, functions, and components in the Student Management System. For additional support or questions, please refer to the source code comments or create an issue in the repository.
