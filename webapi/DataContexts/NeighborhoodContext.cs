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
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Neighborhood to Category relationship
            modelBuilder.Entity<Neighborhood>()
                .HasMany(n => n.Categories)
                .WithOne(c => c.Neighborhood)
                .HasForeignKey(c => c.NeighborhoodId);
            // Category to Post relationship
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Posts)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
            // Post to Comment relationship
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.ParentPost)
                .HasForeignKey(c => c.ParentPostId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Post>()
                .has
        }
    }
}
