using System.Diagnostics.CodeAnalysis;

namespace BlogApi.Data
{
   

    public class AddUserDto
    {
        public string username { get; set; }
        public string email { get; set; }

        public string password { get; set; }

    }

    public class updateUserDto
    {
       
        public  string? username { get; set; }


       
        public string? email { get; set; }


        public string? password { get; set; }

       
        public bool? isActive { get; set; }
    }


    public class LoginDto
    {
        public string usernameOrEmail { get; set; }
        public string password { get; set; }
    }
    public class getUserDetailsDto
    {
        public string username { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime createdAt { get; set; }


    }

    
}
