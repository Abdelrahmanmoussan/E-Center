namespace IdentityText.Utility
{
    public class GradeHelper
    {
        public static string GetGrade(int score, int maxScore)
        {
            if (maxScore == 0)
                return "N/A";

            var percent = (score * 100.0) / maxScore;

            if (percent >= 90)
                return "A+";
            else if (percent >= 85)
                return "A";
            else if (percent >= 80)
                return "B+";
            else if (percent >= 75)
                return "B";
            else if (percent >= 65)
                return "C";
            else if (percent >= 50)
                return "D";
            else
                return "F";
        }
    }
}
