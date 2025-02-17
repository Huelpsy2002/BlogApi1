using BlogApi.Models;

namespace BlogApi.Data
{
    
    
    public class AddBlogDto
    {
        public string title { get; set; }
        public string description { get; set; }
        public List<int>categories { get; set; }
    }
    public class updateBlogDto
    {
        public string title { get; set; }
        public string description { get; set; }



    }


    public class getBlogDto
    {
        public string title { get; set; }
        public string description { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
    public class deleteBlogDto
    {
        public int blogId { get; set; }
    }



    public class getALLBlogsDto
    {
        public string Username { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string>? Catgeories { get; set; }

        public List<CommentDto> Comments { get; set; }
    }
   
}
   
