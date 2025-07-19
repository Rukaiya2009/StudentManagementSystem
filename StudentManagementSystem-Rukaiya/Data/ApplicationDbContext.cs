using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public DbSet<AssignmentReminder> AssignmentReminders { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<ClassroomAnnouncement> ClassroomAnnouncements { get; set; }
        public DbSet<ClassroomMaterial> ClassroomMaterials { get; set; }
        public DbSet<ClassroomMaterialView> ClassroomMaterialViews { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<GradingRule> GradingRules { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<AnnouncementComment> AnnouncementComments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .Property(c => c.Fee)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.GPA)
                .HasPrecision(3, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            // Fix cascade delete for Results → Exams FK
            modelBuilder.Entity<Result>()
                .HasOne(r => r.Exam)
                .WithMany()
                .HasForeignKey(r => r.ExamId)
                .OnDelete(DeleteBehavior.Restrict);  // or DeleteBehavior.NoAction

        }
        public DbSet<StudentManagementSystem.Models.PaymentMethod> PaymentMethod { get; set; } = default!;

    }
}
