using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.Marshalling;

namespace BlogApi.Models
{
   
   public class Users
    {
        [Key]
        public int Id { get; set; }  

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }  

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }

        public string Role { get; set; } = "User";


        public ICollection<Blogs> Blogs { get; set; }
        public ICollection<Comments>Comments { get; set; }


    }


   public class Blogs
    {

        [Key]
        public int id { get; set; }

        [Required, MaxLength(20)]
        public string Title { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;



        public int? UserId { get; set; }
        public Users Users { get; set; }

        public ICollection<Comments> Comments { get; set; }
        public ICollection<BlogsCategories> BlogCategories { get; set; } = new List<BlogsCategories>();


    }

    public class Comments
    {
        [Key]
        public int id { get; set; }
        public string text { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public DateTime updatedAt { get; set; } = DateTime.UtcNow;

        public int? BlogId { get; set; }
        public Blogs Blogs { get; set; }

        
        public int? UserId { get; set; }

        public Users Users { get; set; }
    }


    public class Categories
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }

        public ICollection<Tags> tags { get; set; } = new List<Tags>();
        public ICollection<BlogsCategories> BlogCategories { get; set; } = new List<BlogsCategories>();

    }


    public class BlogsCategories
    {
        public int? BlogId { get; set; }
        public Blogs Blogs { get; set; }

        public int? CategoryId { get; set; }
        public Categories Categories { get; set; }
    }


    public class Tags
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public Categories Categories { get; set; }

    }
}
