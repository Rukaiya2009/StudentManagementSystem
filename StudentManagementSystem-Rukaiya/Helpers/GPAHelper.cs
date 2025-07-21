using StudentManagementSystem_Rukaiya.Models;

public static class GPAHelper
{
    public static decimal CalculateGPA(List<Enrollment> enrollments)
    {
        if (enrollments == null || enrollments.Count == 0) return 0.0M;
        // Example calculation, adjust as needed
        return enrollments.Where(e => e.GPA.HasValue).Select(e => e.GPA!.Value).DefaultIfEmpty(0.0M).Average();
    }
}
