using Microsoft.EntityFrameworkCore;

namespace school_project.Models
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}

