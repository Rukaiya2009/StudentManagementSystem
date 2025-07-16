using StudentManagementSystem.Models;

public static class GPAHelper
{
    public static double CalculateGPA(List<Enrollment> enrollments)
    {
        double totalPoints = 0;
        int count = 0;

        foreach (var e in enrollments)
        {
            if (e.Grade == null) continue;

            switch (e.Grade.ToString())
            {
                case "A": totalPoints += 4; break;
                case "B": totalPoints += 3; break;
                case "C": totalPoints += 2; break;
                case "D": totalPoints += 1; break;
                case "F": totalPoints += 0; break;
            }
            count++;
        }

        return count == 0 ? 0 : totalPoints / count;
    }
}
