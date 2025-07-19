namespace StudentManagementSystem.Data
{
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Result> Results { get; set; }
    public DbSet<GradingRule> GradingRules { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
    public DbSet<AnnouncementComment> AnnouncementComments { get; set; }
    public DbSet<ClassroomMaterialView> ClassroomMaterialViews { get; set; }
    public DbSet<AssignmentReminder> AssignmentReminders { get; set; }
    public DbSet<ClassroomAnnouncement> ClassroomAnnouncements { get; set; }
    public DbSet<ClassroomMaterial> ClassroomMaterials { get; set; }
}
}
