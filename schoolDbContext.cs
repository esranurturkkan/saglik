using Microsoft.EntityFrameworkCore;

namespace school_project.Models
{
    public class schoolDbContext : DbContext
    {
        public schoolDbContext(DbContextOptions<schoolDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

    }
}
