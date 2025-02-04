using BlogApi.Models;

namespace BlogApi.BussinssLogic
{
    public class UserValidation
    {
        private  Users _user;
        public  List<string> errors;
        public UserValidation(Users user)
        {
            _user = user;
            errors = new List<string>();
        }

        public   List<string> Validate()
        {
            

            if (string.IsNullOrWhiteSpace(_user.Username))
                errors.Add("Username is required.");

            if (string.IsNullOrWhiteSpace(_user.Email) || !_user.Email.Contains("@"))
                errors.Add("A valid email is required.");

            if (string.IsNullOrWhiteSpace(_user.PasswordHash) || _user.PasswordHash.Length < 8)
                errors.Add("Password must be at least 8 characters long.");

            return errors;
        }
    }
}
