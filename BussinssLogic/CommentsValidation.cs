using BlogApi.Models;

namespace BlogApi.BussinssLogic
{
    public class CommentsValidation
    {


        private Comments _comment;
        public List<string> errors;
        public CommentsValidation(Comments comment)
        {
            _comment = comment;
            errors = new List<string>();
        }

        public List<string> Validate()
        {

            if (string.IsNullOrWhiteSpace(_comment.text))
                errors.Add("A text is required.");
            if (!string.IsNullOrEmpty(_comment.text) && _comment.text.Length > 50)
            {
                errors.Add("A comment should be less than 20 charechter");
            }
           



            return errors;
        }


    }
}

