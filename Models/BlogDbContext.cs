using System.Data.Entity;

namespace ShauliProject.Models
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<FanClub> Fan { get; set; }

    }
}