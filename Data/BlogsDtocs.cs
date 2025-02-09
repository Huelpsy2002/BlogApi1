namespace BlogApi.Data
{
    
    
    public class AddBlogDto
    {
        public string title { get; set; }
        public string description { get; set; }
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

}
   
