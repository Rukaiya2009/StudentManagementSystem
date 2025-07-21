using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem_Rukaiya.Models;

namespace StudentManagementSystem_Rukaiya.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<StudentManagementSystem_Rukaiya.Models.PaymentMethod> PaymentMethod { get; set; } = default!;

    }

    public static class ApplicationDbContextSeed
    {
        public static void SeedDummyData(ApplicationDbContext context)
        {
            // Only seed if no data exists
            if (!context.Departments.Any())
            {
                var department = new Department { DepartmentName = "Computer Science", Description = "CS Dept", IsActive = true };
                context.Departments.Add(department);
                context.SaveChanges();

                var teacher = new Teacher
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@example.com",
                    FullName = "John Doe",
                    DepartmentId = department.DepartmentId,
                    IsActive = true
                };
                context.Teachers.Add(teacher);
                context.SaveChanges();

                var classroom = new Classroom
                {
                    ClassroomName = "Room 101",
                    DepartmentId = department.DepartmentId,
                    Description = "Main classroom",
                    IsActive = true
                };
                context.Classrooms.Add(classroom);
                context.SaveChanges();

                var student = new Student
                {
                    FirstName = "Alice",
                    LastName = "Smith",
                    Email = "alice.smith@example.com",
                    FullName = "Alice Smith",
                    Gender = Gender.Female,
                    Phone = "1234567890",
                    IsActive = true
                };
                context.Students.Add(student);
                context.SaveChanges();

                var course = new Course
                {
                    CourseName = "Intro to Programming",
                    Title = "CS101",
                    Credit = 3,
                    CourseCode = "CS101",
                    Fee = 100.00M,
                    Level = CourseLevel.Beginner,
                    DepartmentId = department.DepartmentId,
                    TeacherId = teacher.TeacherId,
                    ClassroomId = classroom.ClassroomId,
                    Status = CourseStatus.Paid,
                    IsActive = true
                };
                context.Courses.Add(course);
                context.SaveChanges();
            }
        }
    }
}
