using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.DataContexts
{
    public class NeighborhoodContext: DbContext
    {
        public NeighborhoodContext(DbContextOptions<NeighborhoodContext> options) 
            : base(options) { }

        public DbSet<Neighborhood> Neighborhoods { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
