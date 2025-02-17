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
        public DbSet<Categories> categories { get; set; }
        public DbSet<BlogsCategories> blogsCategories { get; set; }
        public DbSet<Tags>tags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --  blogs-users one-many relationship
            modelBuilder.Entity<Blogs>()
                .HasOne(b => b.Users)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            // --  blogs-users one-many relationship



            // --  Comments-blogs one-many relationship
            modelBuilder.Entity<Comments>()
                .HasOne(c => c.Blogs)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.NoAction);
            // --  Comments-blogs one-many relationship



            // --  Comments-users one-many relationship
            modelBuilder.Entity<Comments>()
                .HasOne(c => c.Users)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            // --  Comments-blogs one-many relationship


            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Users>()
                .HasIndex(e => e.Email)
                .IsUnique();



            // --  blogs-categories many-many relationship
            modelBuilder.Entity<BlogsCategories>().HasKey(bc => new { bc.BlogId, bc.CategoryId });
            modelBuilder.Entity<BlogsCategories>()
                .HasOne(bc => bc.Blogs)
                .WithMany(bc => bc.BlogCategories)
                .HasForeignKey(bc => bc.BlogId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BlogsCategories>()
                .HasOne(bc => bc.Categories)
                .WithMany(bc => bc.BlogCategories)
                .HasForeignKey(bc => bc.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
          

            // --  blogs-categories many to many relationship




            // --  tag-category one-many relationship
            modelBuilder.Entity<Tags>()
                .HasOne(t => t.Categories)
                .WithMany(t => t.tags)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            // --  tag-category one-many relationship
        }

    }
}
