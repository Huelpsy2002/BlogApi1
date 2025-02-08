using BlogApi.Models;

namespace BlogApi.BussinssLogic
{
    public class BlogsValidation
    {


        private Blogs _blog;
        public List<string> errors;
        public BlogsValidation(Blogs blog)
        {
            _blog = blog;
            errors = new List<string>();
        }

        public List<string> Validate()
        {

            if (string.IsNullOrWhiteSpace(_blog.Title))
                errors.Add("A Title is required.");
            if(!string.IsNullOrEmpty(_blog.Title) && _blog.Title.Length > 20)
            {
                errors.Add("A Title should be less than 20 charechter");
            }
            if (string.IsNullOrWhiteSpace(_blog.Description) )
                errors.Add("A Description is required.");

            

            return errors;
        }


    }
}
