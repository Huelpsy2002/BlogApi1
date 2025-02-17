namespace BlogApi.Data
{


    public class CommentDto
    {
        public string Username { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentedAt { get; set; }
    }



    public class AddCommentsDto
    {
        public string text { get; set; }

    }



    public class updateCommentsDto
    {
       
        public string text { get; set; }
    }

    public class getCommentsDto
    {
        public int commentId { get; set; }
        public string text { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int? userId { get; set; }
        public int ?blogId { get; set; }
    }
}
