using Microsoft.EntityFrameworkCore;
using RedditMVP.Models;

namespace RedditMVP.Data;

public class AppDbContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>().HasMany(p => p.Comments).WithOne().HasForeignKey(c => c.PostId);
    }
}
