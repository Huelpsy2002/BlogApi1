using System.ComponentModel.DataAnnotations;

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

        public bool? IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }


        public ICollection<Blogs> Blogs { get; set; }


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

        public DateTime UpdatedAt { get; set; }



        public int UserId { get; set; }
        public Users Users { get; set; }


    }
}
