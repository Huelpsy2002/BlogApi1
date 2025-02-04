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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Blogs>()
                .HasOne(b => b.Users)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Users>()
                .HasIndex(e => e.Email)
                .IsUnique();
        }

    }
}
