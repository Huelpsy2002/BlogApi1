using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BlogApi.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> users { get; set; }
        public DbSet<Blogs> blogs { get; set; }
        public DbSet<Comments> comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Blogs>()
                .HasOne(b => b.Users)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comments>()
                .HasOne(c => c.Blogs)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comments>()
                .HasOne(c => c.Users)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);
               

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Users>()
                .HasIndex(e => e.Email)
                .IsUnique();
        }

    }
}
