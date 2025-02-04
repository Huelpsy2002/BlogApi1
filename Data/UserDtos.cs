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
        [AllowNull]
        public  string? username { get; set; }


        [AllowNull]
        public string? email { get; set; }


        [AllowNull]
        public string? password { get; set; }

        [AllowNull]
        public bool? isActive { get; set; }
    }


    //public class getUserDto
    //{
    //    public string username { get; set; }
    //}
    public class getUserDetailsDto
    {
        public string username { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime createdAt { get; set; }


    }

    
}
