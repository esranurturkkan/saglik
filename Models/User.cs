using System.ComponentModel.DataAnnotations;

namespace school_project.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int? Age { get; set; } // Nullable
        public int? Height { get; set; } // Nullable
        public int? Weight { get; set; } // Nullable
        public int? StepGoal { get; set; } // Nullable
        public int? CalorieGoal { get; set; } // Nullable
        public int? Steps { get; set; } // Nullable
        public int? Calories { get; set; } // Nullable
        public int? ActiveMinutes { get; set; } // Nullable
        public int? Protein { get; set; } // Nullable
        public int? Carbs { get; set; } // Nullable
        public int? Fat { get; set; } // Nullable
        public int? WaterIntake { get; set; } // Nullable
        public int? HeartRate { get; set; } // Nullable
        public string? BloodPressure { get; set; } // Nullable
        public string? Sleep { get; set; } // Nullable
        
    }
}
