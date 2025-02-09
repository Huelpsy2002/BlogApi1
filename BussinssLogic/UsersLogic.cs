using BlogApi.Data;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogApi.BussinssLogic
{

    public interface IUsersLogic
    {
        public Task<(bool success, List<string> Errors)> AddUser(AddUserDto adduserdto);
        public Task<(bool success, List<string> Errors)> UpdateUser(string username, updateUserDto updateuserdto);

        public Task<getUserDetailsDto> getUser(string username);

        public Task<bool> deleteUser(string username);
        public Task<(bool auth, string token)> AuthenticateAsync(LoginDto loginDto);


    }


    public  class UsersLogic:IUsersLogic
    {
        private readonly IConfiguration _configration;
        private readonly AppDbContext _context;
        public UsersLogic(AppDbContext context  , IConfiguration configuration)
        {
            _configration = configuration;
            _context = context;
        }


        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }







        public   async Task<(bool success,List<string>Errors)> AddUser(AddUserDto adduserdto) {

            Users user = new Users { Username = adduserdto.username, Email = adduserdto.email };
            UserValidation uservalidation = new UserValidation(user);



            bool usernameExists = await _context.users.AnyAsync(u => u.Username == adduserdto.username);
            bool emailExists = await _context.users.AnyAsync(u => u.Email == adduserdto.email);
            if (usernameExists)
            {
                uservalidation.errors.Add("username already exist.");
            }
            if (emailExists)
            {
                uservalidation.errors.Add("email already exist.");
            }
            user.PasswordHash = HashPassword(adduserdto.password);

            if (uservalidation.Validate().Count > 0)
            {
                return (false, uservalidation.errors);  
            }
            await  _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return (true, new List<string>());

        }

        public async Task<(bool success, List<string> Errors)> UpdateUser(string username,updateUserDto updateuserdto)
        {

            var user = await _context.users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return (false, new List<string>() { "username does not exist" });
            }

            if (updateuserdto.username != null) {
                
                user.Username = updateuserdto.username;
                //_context.Entry(user).Property(u => u.Username).IsModified = true;



            }
            if (updateuserdto.email != null) { 
                
                user.Email = updateuserdto.email;
                //_context.Entry(user).Property(u => u.Email).IsModified = true;


            }
            if (updateuserdto.isActive != null) {

                if (updateuserdto.isActive==true) { user.IsActive = true; }
                else { user.IsActive = false; }

                //_context.Entry(user).Property(u => u.IsActive).IsModified = true;

            }

            

            UserValidation uservalidation = new UserValidation(user);
            

            if (!string.IsNullOrEmpty(updateuserdto.password))
            {
                if (!VerifyPassword(updateuserdto.password, user.PasswordHash))
                {
                    if ( updateuserdto.password.Length < 8)
                    {
                        uservalidation.errors.Add("Password must be at least 8 characters long.");
                        return (false, uservalidation.errors);

                    }
                    user.PasswordHash = HashPassword(updateuserdto.password);
                }
            }

            if (uservalidation.Validate().Count > 0)
            {
                return (false, uservalidation.errors);
            }

            await _context.SaveChangesAsync();
            return (true, new List<string>());
            



        }

        public async Task<getUserDetailsDto>getUser(string username)
        {
           var user =  await _context.users.FirstOrDefaultAsync(u => u.Username == username);
            return new getUserDetailsDto { username = user.Username, IsActive = user.IsActive,LastLogin=user.LastLogin,createdAt=user.CreatedAt };
        }

        public async Task<bool> deleteUser(string username)
        {
            var user = await _context.users.Include(u=>u.Blogs).Include(u=>u.Comments).
                FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return false;
            }
            if (user.Blogs != null && user.Blogs.Any()) { _context.blogs.RemoveRange(user.Blogs); }
            if (user.Comments != null && user.Comments.Any()) { _context.comments.RemoveRange(user.Comments); }
            _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
            
        }

        public async Task<(bool auth,string token)> AuthenticateAsync(LoginDto loginDto)
        {

             var user = await _context.users
            .FirstOrDefaultAsync(u => u.Username == loginDto.usernameOrEmail || u.Email == loginDto.usernameOrEmail);

            if (user == null || !VerifyPassword(loginDto.password, user.PasswordHash))
            {
                
                return (false,""); // Authentication failed
            }
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            TokenService tokengen = new TokenService(_configration);
            return (true,tokengen.GenerateToken(user));
        }
    }




    }

