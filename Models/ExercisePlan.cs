namespace school_project.NewFolder
{
    public class ExercisePlan
    {
        public int PlanId { get; set; } 
        public int user_id { get; set; }
        public string ExerciseType { get; set; } = "Unknown";
        public string DifficultyLevel { get; set; } = "Medium";
        public string Frequency { get; set; } = "Weekly";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
