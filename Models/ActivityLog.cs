namespace school_project.NewFolder
{
     public class ActivityLog
        {
            public int ActivityId { get; set; } 
            public int user_id { get; set; }
            public string ActivityType { get; set; }
            public int Duration { get; set; } = 0;
            public int Steps { get; set; } = 0;
            public float CaloriesBurned { get; set; } = 0;
            public DateTime Date { get; set; }
        }
    }

